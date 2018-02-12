using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc.TokenHierarchy {
    public class NumberToken : Token {
        public double Value;

        public NumberToken(double v) : base(TokenIdentifier.TokenType.Number) {
            Value = v;
        }

        static bool isDigit(int c) {
            return c >= '0' && c <= '9';
        }

        public static NumberToken Parse(TokenStringReader reader) {
            string num = "";
            int c;
            bool isDec = false;

            if (reader.HasNext()) {
                c = reader.PeekForward();
                if (!isDigit(c)) throw new Exception("Number token not valid at " + reader.Position);
                num += (char)c;
                reader.Forward();
            }

            while (reader.HasNext()) {
                c = reader.PeekForward();

                if (c != '.' && !isDigit(c)) break;

                if (c == '.') {
                    if (isDec) throw new Exception("Number token not valid at " + reader.Position);
                    isDec = true;
                }

                num += (char)c;
                reader.Forward();
            }

            return new NumberToken(double.Parse(num));
        }
    }
}
