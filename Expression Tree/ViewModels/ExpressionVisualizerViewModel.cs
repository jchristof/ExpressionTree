using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expression_Tree.Tree;
using Expression_Tree.ValueProvider;

namespace Expression_Tree.ViewModels {
    class ExpressionVisualizerViewModel {
        public ExpressionVisualizerViewModel() {
            var variables = new Dictionary<string, string> { { "one", "1" } };
            expressionEvaluator = new ExpressionEvaluator(new Variables(variables), new Jurrasic(string.Empty));
        }

        public ExpressionTree expressionTree = new ExpressionTree();
        private readonly ExpressionEvaluator expressionEvaluator;

        private string expression;
        public string Expression {
            get => expression;
            set {
                if (expression == value)
                    return;

                expression = value;
                expressionTree.Parse(expression);
                expressionTree.Evaluate(expressionEvaluator);
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
