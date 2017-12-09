
using Expression_Tree.ValueProvider;

namespace Expression_Tree.Tree {
    public class ExpressionEvaluator {

        public ExpressionEvaluator(IValueProvider variables, IValueProvider javascript) {
            this.variables = variables;
            this.javascript = javascript;
        }

        private readonly IValueProvider variables;
        private readonly IValueProvider javascript;

        public string Evaluate(ExpressionType type, string expression) {
            switch (type) {
                case ExpressionType.eval:
                    return javascript.GetValue(expression);

                case ExpressionType.var:
                    return variables.GetValue(expression);
                    
                case ExpressionType.none:
                    return string.Empty;
            }

            return string.Empty;
        }

    }
}
