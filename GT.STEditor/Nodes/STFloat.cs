using System;
using System.Collections.Generic;
using System.Text;

namespace GTSTEditor.Nodes
{
    public class STFloat : NodeBase
    {
        public STFloat(float val)
        {
            Value = val;
        }

        public float Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
