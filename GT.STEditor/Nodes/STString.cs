using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GTSTEditor.Nodes
{
    [DebuggerDisplay("{Name} ({Child})")]
    public class NodeKey : NodeBase
    {
        public string Name { get; set; }
        public NodeBase Child { get; set; }
    }
}
