using System;

namespace Expression_Tree.Tree {
    public class ExpressionTree {

        public ExpressionTree(ExpressionEvaluator evaluator) {
            _evaluator = evaluator;
        }

        public ExpressionNode _root;
        private readonly ExpressionEvaluator _evaluator;

        /// <summary>
        /// Break the expression into a tree of parts that can each be evaluated
        /// </summary>
        /// <param name="expression"></param>
        public void Parse(string expression) {
            int spanPosition = 0;

            ExpressionNode currentNode = new ExpressionNode();
            _root = currentNode;

            currentNode.StartIndex = 0;
            currentNode.Parent = null;

            bool escaped = false;

            foreach (char character in expression) {
                switch (character) {                            
                    case '[':                    
                        currentNode = currentNode.AddChild();
                        currentNode.Escaped = escaped;
                        currentNode.StartIndex = escaped ? spanPosition - 1 : spanPosition;
                        escaped = false;
                        break;
                    case ']':
                        if (currentNode.Parent == null)
                            break;

                        currentNode.IsExpression = true;
                        currentNode.EndIndex = spanPosition;
                        currentNode.Expression = expression.Substring(currentNode.StartIndex, currentNode.EndIndex - currentNode.StartIndex + 1);

                        currentNode.SubString = new String(currentNode.Expression.ToCharArray());
                        currentNode = currentNode.Parent;
                        break;
                }

                if (character == '\\') {
                    escaped = !escaped;
                }

                spanPosition++;
            }
            do {
                currentNode.EndIndex = spanPosition;
                currentNode.Expression = expression.Substring(currentNode.StartIndex, currentNode.EndIndex - currentNode.StartIndex);
                currentNode.Evaluation = currentNode.Expression;

            } while ((currentNode = currentNode.Parent) != null);
        }

        public string Evaluate() {
            if (_root?.Children == null)
                return string.Empty;

            _root.Evaluation = _root.Expression;

            TraverseEvaluate(_root);
            TraverseResolve(_root);

            return _root.Evaluation;
        }

        private void TraverseResolve(ExpressionNode node) {
            if (node.Children == null)
                return;

            foreach (var n in node.Children)
                if (n.IsExpression)
                    _root.Evaluation = ReplaceFirst(_root.Evaluation, n.SubString, n.Evaluation);
                else
                    TraverseResolve(n);
        }

        //evaluate expression node from bottom to top and left to right
        private void TraverseEvaluate(ExpressionNode node) {
            if (node.Children != null)
                foreach (var n in node.Children)
                    TraverseEvaluate(n);

            if (!node.IsExpression)
                return;

            if (node.Children != null)
                foreach (var n in node.Children)
                    if (n.IsExpression)
                        node.Expression = ReplaceFirst(node.Expression, n.SubString, n.Evaluation);

            node.CompleteExpressionNode(_evaluator);
        }

        static string ReplaceFirst(string text, string search, string replace) {
            if (text == null)
                return null;

            int pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0) {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
