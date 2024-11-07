using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class ClassCreator
    {
        private string _name;
        private bool _isStatic;
        private List<string> _inheritances = new();
        private List<string> _attributes = new();
        private List<FieldCreator> _fields = new();
        private List<MethodCreator> _methods = new();
        private ScriptBuilder _scriptBuilder;

        private ClassCreator(ScriptBuilder scriptBuilder, string name)
        {
            _scriptBuilder = scriptBuilder;
            _name = name;
        }

        public static ClassCreator Create(ScriptBuilder scriptBuilder, string name)
        {
            return new ClassCreator(scriptBuilder, name);
        }

        public ClassCreator AddInheritance(string inheritance)
        {
            _inheritances.Add(inheritance);
            return this;
        }

        public ClassCreator SetStatic()
        {
            _isStatic = true;
            return this;
        }

        public ClassCreator AddAttribute(string attribute)
        {
            _attributes.Add(attribute);
            return this;
        }

        public FieldCreator AddField(string accessModifier, string type, string name)
        {
            var field = FieldCreator.Create(this, accessModifier, type, name);
            _fields.Add(field);
            return field;
        }

        public MethodCreator AddMethod(string name)
        {
            var method = MethodCreator.Create(this, name);
            _methods.Add(method);
            return method;
        }

        public ScriptBuilder End()
        {
            return _scriptBuilder;
        }

        public override string ToString()
        {
            var classBuilder = new StringBuilder();

            foreach (var attribute in _attributes) classBuilder.AppendLine($"    [{attribute}]");
            
            var staticPart = _isStatic ? "static " : "";
            var inheritancePart = _inheritances.Count > 0 ? " : " + string.Join(", ", _inheritances) : "";
            classBuilder.AppendLine($"    public {staticPart}class {_name}{inheritancePart}");
            classBuilder.AppendLine("    {");

            foreach (var field in _fields) classBuilder.Append(field);

            if (_fields.Count > 0 && _methods.Count > 0) classBuilder.AppendLine();

            for (var index = 0; index < _methods.Count; index++)
            {
                var method = _methods[index];
                if (index > 0) classBuilder.AppendLine();
                classBuilder.Append(method);
            }

            classBuilder.AppendLine("    }");

            return classBuilder.ToString();
        }
    }
}