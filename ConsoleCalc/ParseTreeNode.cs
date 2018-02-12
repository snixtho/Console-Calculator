using ConsoleCalc.TokenHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class ParseTreeNode {
        public ParseTreeNode Left;
        public ParseTreeNode Right;
        public Token Token;
    }
}
