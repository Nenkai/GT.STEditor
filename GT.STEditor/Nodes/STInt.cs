using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class STInt : NodeBase
    {
        public STInt(int val)
        {
            Value = val;
        }

        public int Value { get; set; }
        public NodeBase KeyConfigNode { get; set; }
        public override string ToString()
            => Value.ToString();
    }
}
