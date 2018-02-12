using ConsoleCalc.TokenHierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class Lexer {
        TokenStringReader reader;

        public Lexer(TokenStringReader stream) {
            reader = stream;
        }

        /// <summary>
        /// Get string tokens.
        /// </summary>
        /// <returns></returns>
        public TokensContainer Tokenize() {
            TokensContainer tokens = new TokensContainer();

            while (reader.HasNext()) {
                (TokenIdentifier.TokenType type, int advance) = TokenIdentifier.Identify(reader);

                if (reader.PeekForward() == ' ') {
                    reader.Forward();
                } else {
                    for (int i = 0; i < advance; i++) reader.Forward();

                    switch (type) {
                        case TokenIdentifier.TokenType.AndOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.AndOperator));
                            break;
                        case TokenIdentifier.TokenType.DivisionOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.DivisionOperator));
                            break;
                        case TokenIdentifier.TokenType.Identifier:
                            tokens.AddToken(IdentifierToken.Parse(reader));
                            break;
                        case TokenIdentifier.TokenType.LeftParenthesis:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.LeftParenthesis));
                            break;
                        case TokenIdentifier.TokenType.MinusOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.MinusOperator));
                            break;
                        case TokenIdentifier.TokenType.ModuloOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.ModuloOperator));
                            break;
                        case TokenIdentifier.TokenType.MultiplicationOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.MultiplicationOperator));
                            break;
                        case TokenIdentifier.TokenType.NegationOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.NegationOperator));
                            break;
                        case TokenIdentifier.TokenType.Number:
                            tokens.AddToken(NumberToken.Parse(reader));
                            break;
                        case TokenIdentifier.TokenType.BinaryNumber:
                            tokens.AddToken(BinaryNumberToken.Parse(reader));
                            break;
                        case TokenIdentifier.TokenType.OctalNumber:
                            tokens.AddToken(OctalNumberToken.Parse(reader));
                            break;
                        case TokenIdentifier.TokenType.HexNumber:
                            tokens.AddToken(HexNumberToken.Parse(reader));
                            break;
                        case TokenIdentifier.TokenType.OrOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.OrOperator));
                            break;
                        case TokenIdentifier.TokenType.PlussOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.PlussOperator));
                            break;
                        case TokenIdentifier.TokenType.PowerOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.PowerOperator));
                            break;
                        case TokenIdentifier.TokenType.RightParenthesis:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.RightParenthesis));
                            break;
                        case TokenIdentifier.TokenType.ShiftLeftOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.ShiftLeftOperator));
                            break;
                        case TokenIdentifier.TokenType.ShiftRightOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.ShiftRightOperator));
                            break;
                        case TokenIdentifier.TokenType.XorOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.XorOperator));
                            break;
                        case TokenIdentifier.TokenType.ParamSeparator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.ParamSeparator));
                            break;
                        case TokenIdentifier.TokenType.FactorialOperator:
                            tokens.AddToken(new Token(TokenIdentifier.TokenType.FactorialOperator));
                            break;
                        default: throw new Exception("Invalid token at " + reader.Position);
                    }
                }
            }

            return tokens;
        }
    }
}
