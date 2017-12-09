using System;

namespace Expression_Tree.Tree {
    public class ExpressionTree {

        public ExpressionNode Root;

        /// <summary>
        /// Break the expression into a tree of parts that can each be evaluated
        /// </summary>
        /// <param name="expression"></param>
        public void Parse(string expression) {
            int spanPosition = 0;

            ExpressionNode currentNode = new ExpressionNode();
            Root = currentNode;

            currentNode.StartIndex = 0;
            currentNode.Parent = null;
            foreach (char character in expression) {
                switch (character) {
                    case '[':
                        currentNode = currentNode.AddChild();
                        currentNode.StartIndex = spanPosition;
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
                spanPosition++;
            }
            do {
                currentNode.EndIndex = spanPosition;
                currentNode.Expression = expression.Substring(currentNode.StartIndex, currentNode.EndIndex - currentNode.StartIndex);
                currentNode.Evaluation = currentNode.Expression;

            } while ((currentNode = currentNode.Parent) != null);
        }

        static ExpressionNode CompleteExpressionNode(ExpressionNode node) {

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

            return node;
        }

        private ExpressionEvaluator evaluator;

        public string Evaluate(ExpressionEvaluator evaluator) {
            this.evaluator = evaluator;

            Root.Evaluation = Root.Expression;

            TraverseEvaluate(Root);
            TraverseResolve(Root);

            return Root.Evaluation;
        }

        private void TraverseResolve(ExpressionNode node) {
            if (node.Children == null)
                return;

            foreach (var n in node.Children)
                if (n.IsExpression)
                    Root.Evaluation = ReplaceFirst(Root.Evaluation, n.SubString, n.Evaluation);
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

            CompleteExpressionNode(node);

            if (node.ExpressionType != ExpressionType.none)
                node.Evaluation = evaluator.Evaluate(node.ExpressionType, node.RHS);
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
