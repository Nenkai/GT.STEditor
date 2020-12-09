using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor
{
    public abstract class NodeBase
    {
        public NodeType Type { get; set; }
    }

    public enum NodeType
    {
        Null,
        SByte,
        Short,
        Int,
        Long,
        Float,
        MBlob,
        Key,
        StructArray,
        Struct,
        KeyConfig,
        Unk,
        Unk2,
        Unk3,
        UInt = 0x0E,
        ULong = 0x0F,

    }
}
