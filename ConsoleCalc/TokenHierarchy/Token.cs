using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc.TokenHierarchy {
    public class Token {
        TokenIdentifier.TokenType tokenType;
        public TokenIdentifier.TokenType TokenType => tokenType;

        public Token(TokenIdentifier.TokenType type) {
            tokenType = type;
        }
    }
}
