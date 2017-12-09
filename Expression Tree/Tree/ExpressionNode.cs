
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Expression_Tree.Tree {
    [DebuggerDisplay("{Expression}")]
    public class ExpressionNode : INotifyPropertyChanged {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public bool IsExpression { get; set; }
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

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
