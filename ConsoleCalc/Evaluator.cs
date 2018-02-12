using ConsoleCalc.TokenHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class Evaluator {
        public delegate double FunctionDel(params double[] args);

        private Dictionary<string, double> variables;
        private Dictionary<string, EvalFunction> functions;
        private ParseTreeNode root;

        public Evaluator(ParseTreeNode root) {
            variables = new Dictionary<string, double>();
            functions = new Dictionary<string, EvalFunction>();

            this.root = root;

            AddVariable("pi", Math.PI);
            AddVariable("e", Math.E);

            AddFunction(new EvalFunction("abs", 1, (args) => { return Math.Abs(args[0]); }));
            AddFunction(new EvalFunction("acos", 1, (args) => { return Math.Acos(args[0]); }));
            AddFunction(new EvalFunction("asin", 1, (args) => { return Math.Asin(args[0]); }));
            AddFunction(new EvalFunction("atan", 1, (args) => { return Math.Atan(args[0]); }));
            AddFunction(new EvalFunction("atan2", 2, (args) => { return Math.Atan2(args[0], args[1]); }));
            AddFunction(new EvalFunction("ceil", 1, (args) => { return Math.Ceiling(args[0]); }));
            AddFunction(new EvalFunction("round", 1, (args) => { return Math.Round(args[0]); }));
            AddFunction(new EvalFunction("floor", 1, (args) => { return Math.Floor(args[0]); }));
            AddFunction(new EvalFunction("cos", 1, (args) => { return Math.Cos(args[0]); }));
            AddFunction(new EvalFunction("sin", 1, (args) => { return Math.Sin(args[0]); }));
            AddFunction(new EvalFunction("tan", 1, (args) => { return Math.Tan(args[0]); }));
            AddFunction(new EvalFunction("cosh", 1, (args) => { return Math.Cosh(args[0]); }));
            AddFunction(new EvalFunction("sinh", 1, (args) => { return Math.Sinh(args[0]); }));
            AddFunction(new EvalFunction("tanh", 1, (args) => { return Math.Tanh(args[0]); }));
            AddFunction(new EvalFunction("ln", 1, (args) => { return Math.Log(args[0]); }));
            AddFunction(new EvalFunction("log10", 1, (args) => { return Math.Log10(args[0]); }));
            AddFunction(new EvalFunction("log", 1, (args) => { return Math.Log(args[0], 2); }));
            AddFunction(new EvalFunction("logn", 2, (args) => { return Math.Log(args[0], args[1]); }));
            AddFunction(new EvalFunction("max", 2, (args) => { return Math.Max(args[0], args[1]); }));
            AddFunction(new EvalFunction("min", 2, (args) => { return Math.Min(args[0], args[1]); }));
            AddFunction(new EvalFunction("exp", 1, (args) => { return Math.Exp(args[0]); }));
            AddFunction(new EvalFunction("sqrt", 1, (args) => { return Math.Sqrt(args[0]); }));
            AddFunction(new EvalFunction("sqrtn", 2, (args) => { return Math.Pow(args[0], 1/args[2]); }));
            AddFunction(new EvalFunction("lcm", 2, (args) => { return 0; }));
            AddFunction(new EvalFunction("gcd", 2, (args) => { return 0; }));
            AddFunction(new EvalFunction("rand", 1, (args) => { return new Random().Next((int)args[0]); }));
            AddFunction(new EvalFunction("avg", 0, (args) => {
                double sum = 0;
                foreach (double n in args) sum += n;
                return sum / args.Length;
            }));
        }

        public void AddVariable(string name, double value) {
            variables[name] = value;
        }

        public void AddFunction(EvalFunction function) {
            functions[function.Name] = function ?? throw new ArgumentNullException(nameof(function), "The function callback is null.");
        }
        
        private double Evaluate(ParseTreeNode node) {
            if (node is LeftOperatorNode) {
                // left operators
                if (node.Token.TokenType == TokenIdentifier.TokenType.MinusOperator) {
                    // minus
                    return -Evaluate(((LeftOperatorNode)node).Next);
                } else if (node.Token.TokenType == TokenIdentifier.TokenType.NegationOperator) {
                    // negation
                    return ~(long)Evaluate(((LeftOperatorNode)node).Next);
                } else {
                    // factorial
                    double value = Evaluate(((LeftOperatorNode)node).Next);
                    double newValue = 1;
                    while (value > 1) newValue *= value--;
                    return newValue;
                }
            } else if (node is FunctionParseTreeNode) {
                // we have a function
                FunctionParseTreeNode n = (FunctionParseTreeNode)node;
                string name = ((IdentifierToken)n.Token).Identifier;
                if (functions.ContainsKey(name)) {
                    if (n.Parameters.Count < functions[name].MinArgs) {
                        throw new Exception("The function needs at least " + functions[name].MinArgs + " argument(s).");
                    }

                    double[] parValues = new double[n.Parameters.Count];
                    for (int i = 0; i < n.Parameters.Count; i++) {
                        parValues[i] = Evaluate(n.Parameters[i]);
                    }

                    return functions[name].FunctionCallback(parValues);
                } else {
                    throw new Exception("The function '" + name + "' does not exist.");
                }
            } else {
                if (node.Left != null) {
                    // we have an operator
                    double first = Evaluate(node.Left);
                    double second = Evaluate(node.Right);

                    switch (node.Token.TokenType) {
                        case TokenIdentifier.TokenType.MinusOperator: return first - second;
                        case TokenIdentifier.TokenType.PlussOperator: return first + second;
                        case TokenIdentifier.TokenType.MultiplicationOperator: return first * second;
                        case TokenIdentifier.TokenType.DivisionOperator: return first / second;
                        case TokenIdentifier.TokenType.ModuloOperator: return first % second;
                        case TokenIdentifier.TokenType.AndOperator: return (int)first & (int)second;
                        case TokenIdentifier.TokenType.XorOperator: return (int)first ^ (int)second;
                        case TokenIdentifier.TokenType.OrOperator: return (int)first | (int)second;
                        case TokenIdentifier.TokenType.ShiftLeftOperator: return (int)first << (int)second;
                        case TokenIdentifier.TokenType.ShiftRightOperator: return (int)first >> (int)second;
                        case TokenIdentifier.TokenType.PowerOperator: return Math.Pow(first, int.Parse(second.ToString()));
                    }
                } else if (node.Token.TokenType == TokenIdentifier.TokenType.Identifier) {
                    // we have a variable name
                    string name = ((IdentifierToken)node.Token).Identifier;
                    if (variables.ContainsKey(name)) {
                        return variables[name];
                    } else {
                        throw new Exception("There exists no variables with the name '" + name + "'.");
                    }
                }

                return ((NumberToken)node.Token).Value;
            }
        }

        public double GetEvaluation() {
            return Evaluate(root);
        }
    }
}
