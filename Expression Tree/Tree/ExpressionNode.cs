
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Expression_Tree.Tree {
    [DebuggerDisplay("{Expression}")]
    public class ExpressionNode : INotifyPropertyChanged {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public bool IsExpression { get; set; }
        public bool Escaped { get; set; }
        public string Expression { get; set; }
        public string SubString { get; set; }
        public string Evaluation { get; set; }
        public ExpressionType ExpressionType { get; set; }
        public string RHS { get; set; }
        public ExpressionNode Parent { get; set; }
        public List<ExpressionNode> Children { get; set; }

        public ExpressionNode AddChild() {
            if (Children == null)
                Children = new List<ExpressionNode>();

            var newChild = new ExpressionNode { Parent = this };
            Children.Add(newChild);
            return newChild;
        }

        public void CompleteExpressionNode(ExpressionEvaluator evaluator) {

            if (Escaped) {
                Evaluation = SubString.Substring(1);
                IsExpression = true;
                return ;
            }

            string subExpression = Expression.Substring(1, Expression.Length - 2);
            string[] parts = subExpression.Split(new[] { ':' }, 2);

            if (parts.Length != 2) {
                IsExpression = false;
                return;
            }

            Enum.TryParse(parts[0], out ExpressionType expressionType);

            ExpressionType = expressionType;
            RHS = parts[1].Trim();

            if (expressionType == ExpressionType.none)
                IsExpression = false;

            var value = evaluator.Evaluate(ExpressionType, RHS);
            Evaluation = value ?? SubString;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
