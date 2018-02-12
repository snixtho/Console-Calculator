using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalc {
    public class EvalFunction {
        public string Name;
        public int MinArgs;
        public Evaluator.FunctionDel FunctionCallback;

        public EvalFunction(string name, int minArgs, Evaluator.FunctionDel funcCallback) {
            Name = name;
            MinArgs = minArgs;
            FunctionCallback = funcCallback;
        }
    }
}
