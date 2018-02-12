using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc.TokenHierarchy {
    public class BinaryNumberToken {
        static bool isDigit(int c) {
            return c >= '0' && c <= '1';
        }

        public static new NumberToken Parse(TokenStringReader reader) {
            string num = "";
            int c;

            if (reader.HasNext()) {
                c = reader.PeekForward();
                if (!isDigit(c)) throw new Exception("Number token not valid at " + reader.Position);
                num += (char)c;
                reader.Forward();
            }

            while (reader.HasNext()) {
                c = reader.PeekForward();
                if (!isDigit(c)) break;

                num += (char)c;
                reader.Forward();
            }

            return new NumberToken(Convert.ToInt64(num, 2));
        }
    }
}
