
using System.Collections.Generic;
using Expression_Tree.Tree;
using Expression_Tree.ValueProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests {
    [TestClass]
    public class ExpressionTreeTests {

        [TestMethod]
        public void Expressions() {
            var variables = new Dictionary<string, string> {{"one", "1"}};

            var evaluator = new ExpressionEvaluator(new Variables(variables), new Jurrasic(string.Empty));

            var expressionTree = new ExpressionTree();

            expressionTree.Parse("[var:one][eval:[eval:[eval:[var:one] + [var:one]]] + 1]");

            var value = expressionTree.Evaluate(evaluator);

            Assert.IsTrue(value == "13");
        }
    }
}
