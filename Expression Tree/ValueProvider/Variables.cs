
using System.Collections.Generic;

namespace Expression_Tree.ValueProvider {
    public class Variables : IValueProvider {

        public Variables(IDictionary<string, string> variables) {
            this.variables = variables ?? new Dictionary<string, string>();
        }

        private readonly IDictionary<string, string> variables;
        public string GetValue(string expression) {
            return variables.ContainsKey(expression) ? variables[expression] : null;
        }

    }
}
