using ConsoleCalc.TokenHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class FunctionParseTreeNode : ParseTreeNode {
        public List<ParseTreeNode> Parameters = new List<ParseTreeNode>();
    }
}
