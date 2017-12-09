
using System;
using System.Diagnostics;
using Jurassic;

namespace Expression_Tree.ValueProvider {
    public class Jurrasic : IValueProvider {

        public Jurrasic(string javascript) {
            engine = new ScriptEngine();

            try {
                ScriptSource scriptSource = new StringScriptSource(javascript);
                engine.Execute(scriptSource);
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        private readonly ScriptEngine engine;

        public string GetValue(string expression) {
            object value = engine.Evaluate(expression);

            bool? boolean = value as bool?;
            //hack - true.ToString() => "True" which is a string in js
            //this forces out lowercase "true" which maintains the boolean type for follow on js evaluations
            return (boolean != null ? boolean.ToString().ToLower() : value).ToString();
        }

    }
}
