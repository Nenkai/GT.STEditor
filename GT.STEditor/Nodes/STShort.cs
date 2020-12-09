using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class STShort : NodeBase
    {
        public STShort(short val)
        {
            Value = val;
        }

        public short Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
