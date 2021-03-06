﻿
using System.Collections.Generic;
using Expression_Tree.Tree;
using Expression_Tree.ValueProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests {
    [TestClass]
    public class ExpressionTreeTests {

        [TestMethod]
        public void ExpressionTreeTests_Valid_Expressions() {
            var variables = new Dictionary<string, string> {{"one", "1"}};
            var evaluator = new ExpressionEvaluator(new Variables(variables), new Jurrasic(string.Empty));
            var expressionTree = new ExpressionTree(evaluator);

            expressionTree.Parse("[var:one][eval:[eval:[eval:[var:one] + [var:one]]] + 1]");

            var value = expressionTree.Evaluate();

            Assert.IsTrue(value == "13");

            expressionTree.Parse("[var:1]");

            value = expressionTree.Evaluate();

            Assert.IsTrue(value == "[var:1]");
        }

        [TestMethod]
        public void ExpressionTreeTests_Invalid_Variable() {
            var variables = new Dictionary<string, string> { { "one", "1" } };
            var evaluator = new ExpressionEvaluator(new Variables(variables), new Jurrasic(string.Empty));
            var expressionTree = new ExpressionTree(evaluator);

            expressionTree.Parse("[var:1]");

            var value = expressionTree.Evaluate();

            //no variable by this name - returns original expression
            Assert.IsTrue(value == "[var:1]");
        }

        [TestMethod]
        public void ExpressionTreeTests_Escaped_Expression_Sequence() {
            var variables = new Dictionary<string, string> { { "one", "1" } };
            var evaluator = new ExpressionEvaluator(new Variables(variables), new Jurrasic(string.Empty));
            var expressionTree = new ExpressionTree(evaluator);

            expressionTree.Parse("[var:one]");
            var value = expressionTree.Evaluate();

            Assert.IsTrue(value == "1");

            expressionTree.Parse(@"\[var:one]");
            value = expressionTree.Evaluate();
            Assert.IsTrue(value == "[var:one]");

            expressionTree.Parse(@"\\[var:one]");
            value = expressionTree.Evaluate();
            Assert.IsTrue(value == @"\\1");

            expressionTree.Parse(@"\\[var:one]\\[var:one]");
            value = expressionTree.Evaluate();
            Assert.IsTrue(value == @"\\1\\1");
        }

        [TestMethod]
        public void ExpressionTreeTests_ChangeUnderlingVariables() {
            var variables = new Dictionary<string, string> {{"one", "1"}};
            var evaluator = new ExpressionEvaluator(new Variables(variables), new Jurrasic(string.Empty));
            var expressionTree = new ExpressionTree(evaluator);
            expressionTree.Parse("[var:one]");

            var value = expressionTree.Evaluate();

            Assert.IsTrue(value == "1");

            variables.Remove("one");
            variables["one"] = "2";


            value = expressionTree.Evaluate();

            Assert.IsTrue(value == "2");
        }

        [TestMethod]
        public void ExpressionTreeTests_FailingTests() {
            var variables = new Dictionary<string, string> { { "one", "1" } };
            var evaluator = new ExpressionEvaluator(new Variables(variables), new Jurrasic(string.Empty));
            var expressionTree = new ExpressionTree(evaluator);
            expressionTree.Parse(@"\[var:one][var:one]");

            var value = expressionTree.Evaluate();
            
            Assert.IsTrue(value == "[var:one]1");
        }
    }
}
