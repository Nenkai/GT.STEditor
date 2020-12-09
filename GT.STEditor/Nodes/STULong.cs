using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class STULong : NodeBase
    {
        public STULong(ulong val)
        {
            Value = val;
        }

        public ulong Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
