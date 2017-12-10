
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

        public static ExpressionNode CompleteExpressionNode(ExpressionNode node, ExpressionEvaluator evaluator) {

            if (node.Escaped) {
                node.Evaluation = node.SubString.Substring(1);
                node.IsExpression = true;
                return node;
            }

            string subExpression = node.Expression.Substring(1, node.Expression.Length - 2);
            string[] parts = subExpression.Split(new[] { ':' }, 2);

            if (parts.Length != 2) {
                node.IsExpression = false;
                return node;
            }

            Enum.TryParse(parts[0], out ExpressionType expressionType);

            node.ExpressionType = expressionType;
            node.RHS = parts[1].Trim();

            if (expressionType == ExpressionType.none)
                node.IsExpression = false;

            var value = evaluator.Evaluate(node.ExpressionType, node.RHS);
            node.Evaluation = value ?? node.SubString;

            return node;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
