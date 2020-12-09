using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class STLong : NodeBase
    {
        public STLong(long val)
        {
            Value = val;
        }

        public long Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
