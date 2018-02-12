using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class TokenIdentifier {
        public enum TokenType {
            Number,
            BinaryNumber,
            OctalNumber,
            HexNumber,
            Identifier,
            LeftParenthesis,
            RightParenthesis,
            PowerOperator,
            MultiplicationOperator,
            DivisionOperator,
            ModuloOperator,
            AndOperator,
            XorOperator,
            ShiftLeftOperator,
            ShiftRightOperator,
            MinusOperator,
            NegationOperator,
            PlussOperator,
            OrOperator,
            ParamSeparator,
            AssignmentOperator,
            FactorialOperator,
            Unknown
        }

        /// <summary>
        /// Identify a token for its type.
        /// </summary>
        /// <param name="reader">The reader with the wanted state to use for checking.</param>
        /// <returns>The token type and advancement value.</returns>
        public static (TokenType, int) Identify(TokenStringReader reader) {
            TokenType type = TokenType.Unknown;
            int advance = 1; // amount of characters to advance for a token, for value parsing; advance the needed chars to get to the actual value
            
            if (IsBinaryNumber(reader)) { type = TokenType.BinaryNumber; advance = 2; }
            else if (IsOctalNumber(reader)) { type = TokenType.OctalNumber; advance = 2; }
            else if (IsHexNumber(reader)) { type = TokenType.HexNumber; advance = 2; }
            else if (IsNumber(reader)) { type = TokenType.Number; advance = 0; } 
            else if (IsLeftParenthesis(reader)) { type = TokenType.LeftParenthesis; }
            else if (IsRightParenthesis(reader)) { type = TokenType.RightParenthesis; }
            else if (IsPowerOperator(reader)) { type = TokenType.PowerOperator; }
            else if (IsMultiplicationOperator(reader)) { type = TokenType.MultiplicationOperator; }
            else if (IsDivisionOperator(reader)) { type = TokenType.DivisionOperator; }
            else if (IsModuloOperator(reader)) { type = TokenType.ModuloOperator; }
            else if (IsAndOperator(reader)) { type = TokenType.AndOperator; }
            else if (IsXorOperator(reader)) { type = TokenType.XorOperator; }
            else if (IsShiftLeftOperator(reader)) { type = TokenType.ShiftLeftOperator; advance = 2; }
            else if (IsShiftRightOperator(reader)) { type = TokenType.ShiftRightOperator; advance = 2; }
            else if (IsMinusOperator(reader)) { type = TokenType.MinusOperator; }
            else if (IsNegationOperator(reader)) { type = TokenType.NegationOperator; }
            else if (IsPlussOperator(reader)) { type = TokenType.PlussOperator; }
            else if (IsOrOperator(reader)) { type = TokenType.OrOperator; }
            else if (IsParamSeparator(reader)) { type = TokenType.ParamSeparator; }
            else if (IsAssignmentOperator(reader)) { type = TokenType.AssignmentOperator; }
            else if (IsFactorialOperator(reader)) { type = TokenType.FactorialOperator; }
            else if (IsIdentifier(reader)) { type = TokenType.Identifier; advance = 0; }

            return (type, advance);
        }
        
        /****************************/

        static bool IsNumber(TokenStringReader reader) {
            if (!reader.HasNext()) return false;
            int c = reader.PeekForward();
            return c >= 48 && c <= 57;
        }

        static bool IsBinaryNumber(TokenStringReader reader) {
            if (!reader.HasNext(3)) return false;
            int c = reader.PeekForward(2);
            return reader.PeekForward() == '0' && reader.PeekForward(1) == 'b' && (c == '0' || c == '1');
        }

        static bool IsOctalNumber(TokenStringReader reader) {
            if (!reader.HasNext(3)) return false;
            int c = reader.PeekForward(2);
            return reader.PeekForward() == '0' && reader.PeekForward(1) == 'o' && (c >= '0' && c <= '7');
        }

        static bool IsHexNumber(TokenStringReader reader) {
            if (!reader.HasNext(3)) return false;
            int c = reader.PeekForward(2);
            return reader.PeekForward() == '0' && reader.PeekForward(1) == 'x' && ((c >= '0' && c <= '7') || (c >= 'A' && c <= 'F'));
        }

        static bool IsLeftParenthesis(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '(';
        }

        static bool IsRightParenthesis(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == ')';
        }

        static bool IsPowerOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '^';
        }

        static bool IsMultiplicationOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '*';
        }
        
        static bool IsDivisionOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '/';
        }

        static bool IsModuloOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '%';
        }

        static bool IsAndOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '&';
        }

        static bool IsXorOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '@';
        }

        static bool IsShiftLeftOperator(TokenStringReader reader) {
            return reader.HasNext(2) && reader.PeekForward() == '<' && reader.PeekForward(1) == '<';
        }

        static bool IsShiftRightOperator(TokenStringReader reader) {
            return reader.HasNext(2) && reader.PeekForward() == '>' && reader.PeekForward(1) == '>';
        }

        static bool IsMinusOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '-';
        }

        static bool IsNegationOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '~';
        }

        static bool IsPlussOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '+';
        }

        static bool IsOrOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '|';
        }

        static bool IsIdentifier(TokenStringReader reader) {
            if (!reader.HasNext()) return false;

            int c = reader.PeekForward();
            return !(c >= 48 && c <= 57) && (c == '_' || (c >= 65 && c <= 90) || (c >= 97 && c <= 122));
        }

        static bool IsParamSeparator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == ',';
        }

        static bool IsAssignmentOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '=';
        }

        static bool IsFactorialOperator(TokenStringReader reader) {
            return reader.HasNext() && reader.PeekForward() == '!';
        }
    }
}
