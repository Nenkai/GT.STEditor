using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Syroot.BinaryData;
using Syroot.BinaryData.Memory;

using GTSTEditor.Nodes;

namespace GTSTEditor
{
    public class SerializedTree
    {
        public string[] NodeNames;
        public NodeBase RootNode { get; set; }

        public static SerializedTree Read(string file)
        {
            var sr = new SpanReader(File.ReadAllBytes(file), Syroot.BinaryData.Core.Endian.Big);
            if (sr.ReadInt32() == 0x18)
                sr.Position = 0x10;
            else
                sr.Position = 0;
            return Read(ref sr);
        }

        public static SerializedTree Read(ref SpanReader sr)
        {
            int basePos = sr.Position;

            sr.ReadByte(); // 0E
            int startPos = sr.ReadInt32();

            int baseTreeOffset = sr.Position;
            sr.Position = basePos + startPos;

            var tree = new SerializedTree();
            tree.NodeNames = new string[sr.Read7BitUInt32()];
            for (int i = 0; i < tree.NodeNames.Length; i++)
                tree.NodeNames[i] = sr.ReadString1();

            sr.Position = baseTreeOffset;
            tree.RootNode = tree.ReadNode(ref sr, null);

            return tree;
        }

        private NodeBase ReadNode(ref SpanReader sr, NodeBase parent)
        {
            NodeBase currentNode = null;

            var nodeType = (NodeType)sr.ReadByte();
            switch (nodeType)
            {
                case NodeType.Null:
                    currentNode = new STObjectNull();
                    break;
                case NodeType.Short:
                    currentNode = new STShort(sr.ReadInt16());
                    break;
                case NodeType.SByte:
                    currentNode = new STSByte(sr.ReadSByte());
                    break;
                case NodeType.Int:
                    if (parent.Type == NodeType.Struct && parent.Type != NodeType.Int) // For key_config
                    {
                        currentNode = new STInt(sr.ReadInt32());
                        (currentNode as STInt).KeyConfigNode = ReadNode(ref sr, currentNode);
                    }
                    else
                        currentNode = new STInt(sr.ReadInt32());
                    break;
                case NodeType.UInt:
                    currentNode = new STUInt(sr.ReadUInt32());
                    break;
                case NodeType.Float:
                    currentNode = new STFloat(sr.ReadSingle());
                    break;
                case NodeType.Long:
                    currentNode = new STLong(sr.ReadInt64());
                    break;
                case NodeType.ULong:
                    currentNode = new STULong(sr.ReadUInt64());
                    break;
                case NodeType.MBlob:
                    currentNode = new MBlob(sr.ReadBytes(sr.ReadInt32()));
                    break;
                case NodeType.Struct:
                    currentNode = new STStruct();
                    currentNode.Type = nodeType;
                    var nodeStruct = currentNode as STStruct;
                    int childCount = sr.ReadInt32();

                    nodeStruct.Children = new List<NodeBase>(childCount);
                    for (int i = 0; i < childCount; i++)
                        nodeStruct.Children.Add(ReadNode(ref sr, currentNode));
                    break;
                case NodeType.StructArray:
                    currentNode = new STArray();

                    currentNode.Type = nodeType;
                    var nodeArrStruct = currentNode as STArray;
                    int childArrCount = sr.ReadInt32();

                    nodeArrStruct.Children = new List<NodeBase>(childArrCount);
                    for (int i = 0; i < childArrCount; i++)
                        nodeArrStruct.Children.Add(ReadNode(ref sr, currentNode));
                    break;
                case NodeType.Key:
                    currentNode = new NodeKey();
                    currentNode.Type = nodeType;
                    var nodeKey = currentNode as NodeKey;
                    nodeKey.Name = NodeNames[sr.Read7BitUInt32()];

                    if (parent.Type == NodeType.Struct && parent.Type != NodeType.Key)
                        nodeKey.Child = ReadNode(ref sr, currentNode);
                    break;
                case NodeType.KeyConfig:
                    currentNode = new STStruct2();
                    currentNode.Type = nodeType;
                    (currentNode as STStruct2).Child = ReadNode(ref sr, currentNode);
                    break;
                default:
                    throw new Exception($"Unexpected structure type {nodeType}");
            }

            currentNode.Type = nodeType;
            return currentNode;
        }

        public void Save(string path)
        {
            using (var bs = new BinaryStream(new FileStream(path, FileMode.Create)))
            {
                bs.ByteConverter = ByteConverter.Big;
                bs.WriteInt32(0x18);
                bs.WriteInt64(80009560400);
                bs.Position += 4;

                bs.WriteByte(0x0E);
                bs.Position += 4; // Length
                List<string> keys = new List<string>();
                WriteNode(bs, RootNode, ref keys);
                int keyTableOffset = (int)bs.Length;
                bs.EncodeAndAdvance((uint)keys.Count);
                foreach (var key in keys)
                    bs.WriteString(key, StringCoding.ByteCharCount);
                int totalLen = (int)bs.Position;

                bs.Position = 0x10 + 1;
                bs.WriteInt32(keyTableOffset - 0x10);

                bs.Position = 0x0C;
                bs.WriteInt32(totalLen - 0x10);
            }
        }

        private void WriteNode(BinaryStream bs, NodeBase node, ref List<string> keys)
        {
            bs.WriteByte((byte)node.Type);
            switch (node)
            {
                case STObjectNull n:
                    break;

                case STSByte @sbyte:
                    bs.WriteSByte(@sbyte.Value); break;
                case STByte @byte:
                    bs.WriteByte(@byte.Value); break;
                case STShort @short:
                    bs.WriteInt16(@short.Value); break;
                case STInt @int:
                    bs.WriteInt32(@int.Value);
                    if (@int.KeyConfigNode != null)
                        WriteNode(bs, @int.KeyConfigNode, ref keys);
                    break;
                case STUInt @uint:
                    bs.WriteUInt32(@uint.Value); break;
                case STLong @long:
                    bs.WriteInt64(@long.Value); break;
                case STULong @ulong:
                    bs.WriteUInt64(@ulong.Value); break;
                case STFloat @float:
                    bs.WriteSingle(@float.Value); break;

                case MBlob @blob:
                    bs.WriteInt32(blob.Data.Length);
                    bs.Write(blob.Data.ToArray());
                    break;

                case NodeKey key:
                    if (!keys.Contains(key.Name))
                        keys.Add(key.Name);
                    int index = keys.IndexOf(key.Name);
                    bs.EncodeAndAdvance((uint)index);
                    if (key.Child != null)
                        WriteNode(bs, key.Child, ref keys);
                    break;
                case STStruct @struct:
                    bs.WriteInt32(@struct.Children.Count);
                    foreach (var child in @struct.Children)
                        WriteNode(bs, child, ref keys);
                    break;
                case STStruct2 @struct2:
                    WriteNode(bs, struct2.Child, ref keys);
                    break;
                case STArray @arr:
                    bs.WriteInt32(@arr.Children.Count);
                    foreach (var child in @arr.Children)
                        WriteNode(bs, child, ref keys);
                    break;
                default:
                    throw new Exception("Missing");
            }
        }

    }
}
