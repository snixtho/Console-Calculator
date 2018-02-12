using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class Parser {
        TokensContainer tokens;

        public Parser(TokensContainer tokens) {
            this.tokens = tokens;
        }

        public ParseTreeNode BuildParseTree() {
            ParseTreeNode root = Expr();
            return root;
        }

        /*************************/

        /*
            (* The grammar assumes any amount of whitespaces between each token *)

            digit = ? 0-9 ?;
            binaryDigit = ? 0-1 ? ;
            octalDigit = ? 0-7 ? ;
            hexDigit = ? 0-9A-F ? ;
            letter = ? a-zA-Z ? ;

            identifier = ( "_" | letter ) { "_" | letter | digit } ;
            functionName = identifier ;
            constant = identifier ;

            number = digit { digit } [ "." digit { digit } ] ;
            binaryNumber = "0b" binaryNumber { binaryNumber } ;
            octalNumber = "0o" octalDigit { octalDigit } ;
            hexNumber = "0x" hexDigit { hexDigit } ;

            functionArguments = expr { "," expr } ;
            functionCall = functionName "(" [ functionArguments ] ")" ;
            factorBase = number | "(" expr ")" | functionCall | constant | binaryNumber | octalNumber | hexNumber;
            factor = [ "-" | "~" ] factorBase { "^" factor } [ "!" ];
            term = factor { ( "*" | "/" | "%" | "&" | "@" | "<<" | ">>" ) factor } ;
            expr = term { ( "+" | "-" | "|" )  term } (* | identifier "=" term *) ;
            
        */

        ParseTreeNode Expr(bool expectParenthesis=false) {
            ParseTreeNode node = new ParseTreeNode();
            if (tokens.HasNext()) {
                node.Left = Term();
            } else {
                return null;
            }

            if (tokens.HasNext() && (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.PlussOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.MinusOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.OrOperator)) {
                node.Token = tokens.Forward();
                node.Right = Term();

                while (tokens.HasNext() && (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.PlussOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.MinusOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.OrOperator)) {
                    ParseTreeNode newNode = new ParseTreeNode();

                    newNode.Token = tokens.Forward();
                    newNode.Left = node;
                    newNode.Right = Term();
                    node = newNode;
                }
            }

            if (expectParenthesis && (!tokens.HasNext() || tokens.PeekForward().TokenType != TokenIdentifier.TokenType.RightParenthesis)) {
                throw new Exception("Syntax error: Expecting ')' at token " + tokens.Position);
            } else if (expectParenthesis && tokens.HasNext() && tokens.PeekForward().TokenType == TokenIdentifier.TokenType.RightParenthesis) {
                tokens.Forward();
            }

            if (node.Right == null) {
                node = node.Left;
            }

            return node;
        }

        ParseTreeNode Term() {
            ParseTreeNode node = new ParseTreeNode();

            if (!tokens.HasNext()) {
                throw new Exception("Syntax error: Expecting factor at token " + tokens.Position);
            }

            node.Left = Factor();

            if (tokens.HasNext() && (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.MultiplicationOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.DivisionOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.ModuloOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.AndOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.XorOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.ShiftLeftOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.ShiftRightOperator)) {
                node.Token = tokens.Forward();
                node.Right = Factor();

                while (tokens.HasNext() && (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.MultiplicationOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.DivisionOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.ModuloOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.AndOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.XorOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.ShiftLeftOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.ShiftRightOperator)) {
                    ParseTreeNode newNode = new ParseTreeNode();

                    newNode.Token = tokens.Forward();
                    newNode.Left = node;
                    newNode.Right = Factor();
                    node = newNode;
                }
            }
            
            if (node.Right == null) {
                node = node.Left;
            }

            return node;
        }

        ParseTreeNode Factor() {
            ParseTreeNode node = new ParseTreeNode();

            if (!tokens.HasNext()) {
                throw new Exception("Syntax error: Expecting factor base at token " + tokens.Position);
            }

            LeftOperatorNode leftOpt = null;
            if (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.MinusOperator
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.NegationOperator) {
                leftOpt = new LeftOperatorNode();
                leftOpt.Token = tokens.Forward();
            }

            node.Left = FactorBase();

            if (tokens.HasNext() && tokens.PeekForward().TokenType == TokenIdentifier.TokenType.PowerOperator) {
                node.Token = tokens.Forward();
                node.Right = Factor();

                while (tokens.HasNext() && tokens.PeekForward().TokenType == TokenIdentifier.TokenType.PowerOperator) {
                    ParseTreeNode newNode = new ParseTreeNode();

                    newNode.Token = tokens.Forward();
                    newNode.Left = node;
                    newNode.Right = Factor();
                    node = newNode;
                }
            }

            if (node.Right == null) {
                node = node.Left;
            }

            if (leftOpt != null) {
                leftOpt.Next = node;
                node = leftOpt;
            }

            if (tokens.HasNext() && tokens.PeekForward().TokenType == TokenIdentifier.TokenType.FactorialOperator) {
                LeftOperatorNode newNode = new LeftOperatorNode();
                newNode.Token = tokens.PeekForward();
                newNode.Next = node;
                node = newNode;
            }

            return node;
        }

        ParseTreeNode FactorBase() {
            if (!tokens.HasNext()) {
                throw new Exception("Syntax error: Expecting token at token " + tokens.Position);
            }

            if (tokens.HasNext(2)
                && tokens.PeekForward().TokenType == TokenIdentifier.TokenType.Identifier
                && tokens.PeekForward(1).TokenType == TokenIdentifier.TokenType.LeftParenthesis) {
                return FunctionCall();
            } else if (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.Number
                || tokens.PeekForward().TokenType == TokenIdentifier.TokenType.Identifier) {
                return new ParseTreeNode {
                    Token = tokens.Forward()
                };
            } else if (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.LeftParenthesis) {
                tokens.Forward();
                return Expr(true);
            } 

            throw new Exception("Syntax error: Expecting number, expression, function call or constant at token " + tokens.Position);
        }

        ParseTreeNode FunctionCall() {
            FunctionParseTreeNode node = new FunctionParseTreeNode();

            if (!tokens.HasNext()) {
                throw new Exception("Synax error: Expecting function identifieri at token " + tokens.Position);
            }

            node.Token = tokens.Forward();

            if (!tokens.HasNext() || tokens.PeekForward().TokenType != TokenIdentifier.TokenType.LeftParenthesis) {
                throw new Exception("Synax error: Expecting function identifieri at token " + tokens.Position);
            }

            tokens.Forward();

            bool expect = false;
            while (tokens.HasNext()) {
                if (tokens.PeekForward().TokenType == TokenIdentifier.TokenType.RightParenthesis) {
                    tokens.Forward();
                    return node;
                }

                if (expect && tokens.PeekForward().TokenType == TokenIdentifier.TokenType.ParamSeparator) {
                    tokens.Forward();
                    expect = false;
                } else {
                    node.Parameters.Add(Expr());
                    expect = true;
                }
            }

            throw new Exception("Syntax error: Expecting function end ')' at token " + tokens.Position);
        }
    }
}
