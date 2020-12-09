using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class STSByte : NodeBase
    {
        public STSByte(sbyte val)
        {
            Value = val;
        }

        public sbyte Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
