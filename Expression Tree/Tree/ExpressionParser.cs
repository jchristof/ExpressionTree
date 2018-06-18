
using System.Collections.Generic;

namespace Expression_Tree.Tree {

    public class BracketPos {
        public int pos;
        public bool left;

        public BracketPos(int pos, bool left) {
            this.pos = pos;
            this.left = left;
        }
    }


    public class ExpressionParser {
        public static List<string> ParseBrackets(string expression) {
            List<string> matches = new List<string>();
            List<BracketPos> brackets = new List<BracketPos>();

            for (int i = 0; i < expression.Length; i++) {
                if (expression[i] == '[') {
                    brackets.Add(new BracketPos(i, true));
                }
                else if (expression[i] == ']') {
                    brackets.Add(new BracketPos(i, false));
                }
            }

            bool done = false;
            while (!done && brackets.Count > 0) {
                done = true;
                for (int i = 0; i < brackets.Count - 1; i++) {
                    if (brackets[i].left && !(brackets[i + 1].left)) {
                        int start = brackets[i].pos;
                        int length = (brackets[i + 1].pos - start) + 1;
                        matches.Add(expression.Substring(start, length).Replace("\r", string.Empty).Replace("\n", string.Empty));
                        brackets.Remove(brackets[i + 1]);
                        brackets.Remove(brackets[i]);
                        done = false;
                        break;
                    }
                }
            }

            return matches;
        }
    }
}
