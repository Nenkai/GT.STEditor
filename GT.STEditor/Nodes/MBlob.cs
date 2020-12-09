using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class MBlob : NodeBase
    {
        public MBlob(Memory<byte> data)
        {
            Data = data;
        }
        public Memory<byte> Data { get; set; }
    }
}
