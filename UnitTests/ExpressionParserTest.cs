
using Expression_Tree.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests {
    [TestClass]
    public class ExpressionParserTest {
        [TestMethod]
        public void ParseTest() {
            var expression = "[var:one][eval:[eval:[eval:[var:one] + [var:one]]] + 1]";
            var brackets = ExpressionParser.ParseBrackets(expression);
        }
    }
}
