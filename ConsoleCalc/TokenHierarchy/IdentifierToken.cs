using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc.TokenHierarchy {
    public class IdentifierToken : Token {
        public string Identifier;

        public IdentifierToken(string s) : base(TokenIdentifier.TokenType.Identifier) {
            Identifier = s;
        }

        static bool validFirstChar(int c) {
            return !(c >= 48 && c <= 57) && (c == '_' || (c >= 65 && c <= 90) || (c >= 97 && c <= 122));
        }

        static bool validNameChar(int c) {
            return (c >= 48 && c <= 57) || c == '_' || (c >= 65 && c <= 90) || (c >= 97 && c <= 122);
        }

        public static IdentifierToken Parse(TokenStringReader reader) {
            string id = "";
            int c;

            if (reader.HasNext()) {
                c = reader.PeekForward();
                if (!validFirstChar(c)) throw new Exception("Name token not valid at " + reader.Position);
                id += (char)c;
                reader.Forward();
            } else {
                throw new Exception("Unexpected end of data.");
            }

            while (reader.HasNext()) {
                c = reader.PeekForward();

                if (!validNameChar(c)) break;
                id += (char)c;

                reader.Forward();
            }

            return new IdentifierToken(id);
        }
    }
}
