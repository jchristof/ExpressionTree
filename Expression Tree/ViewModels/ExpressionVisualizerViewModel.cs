
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expression_Tree.Tree;
using Expression_Tree.ValueProvider;

namespace Expression_Tree.ViewModels {
    class ExpressionVisualizerViewModel : INotifyPropertyChanged{

        public ExpressionVisualizerViewModel(Dictionary<string, string> savedVars) {
            Variables = new ObservableCollection<DataVariable>();
            expressionVariables = savedVars;

            foreach (var v in expressionVariables) {
                Variables.Add(new DataVariable{Key = v.Key, Value = v.Value});
            }
            
            expressionEvaluator = new ExpressionEvaluator(new Variables(expressionVariables), new Jurrasic(string.Empty));
        }

        private readonly Dictionary<string, string> expressionVariables;
        public Dictionary<string, string> ExpressionVariables => expressionVariables;

        public ExpressionTree expressionTree = new ExpressionTree();
        private readonly ExpressionEvaluator expressionEvaluator;

        public class DataVariable {
            public DataVariable() { }
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public ObservableCollection<DataVariable> Variables { get; set; }

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

        public void RowAdded(object variable) {
            if(!(variable is DataVariable v) || v.Key == null)
                return;

            if (expressionVariables.ContainsKey(v.Key))
                Variables.Remove(v);

            expressionVariables[v.Key] = v.Value;
            expressionTree.Evaluate(expressionEvaluator);
            RaisePropertyChanged("Expression");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
