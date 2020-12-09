using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class STByte : NodeBase
    {
        public STByte(byte val)
        {
            Value = val;
        }

        public byte Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
