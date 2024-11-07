using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class MethodCreator
    {
        private string _name;
        private bool _isStatic;
        private ClassCreator _classCreator;
        private List<string> _lines = new();
        private List<string> _params = new();
        private List<string> _attributes = new();

        private MethodCreator(ClassCreator classCreator, string name)
        {
            _classCreator = classCreator;
            _name = name;
        }

        public static MethodCreator Create(ClassCreator classCreator, string name)
        {
            return new MethodCreator(classCreator, name);
        }

        public MethodCreator AddParam(string type, string name)
        {
            _params.Add($"{type} {name}");
            return this;
        }

        public MethodCreator SetStatic()
        {
            _isStatic = true;
            return this;
        }

        public MethodCreator AddAttribute(string attribute)
        {
            _attributes.Add(attribute);
            return this;
        }

        public MethodCreator AddLine(string line)
        {
            _lines.Add(line);
            return this;
        }

        public MethodCreator AddSpace()
        {
            _lines.Add("");
            return this;
        }

        public ClassCreator End()
        {
            return _classCreator;
        }

        public override string ToString()
        {
            var methodBuilder = new StringBuilder();

            foreach (var attribute in _attributes)
            {
                methodBuilder.AppendLine($"        [{attribute}]");
            }

            var staticPart = _isStatic ? "static " : "";
            var paramsList = _params.Count > 0 ? string.Join(", ", _params) : "";
            methodBuilder.AppendLine($"        public {staticPart}void {_name}({paramsList})");
            methodBuilder.AppendLine("        {");

            foreach (var line in _lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    methodBuilder.AppendLine();
                }
                else
                {
                    methodBuilder.AppendLine($"            {line}");
                }
            }

            methodBuilder.AppendLine("        }");

            return methodBuilder.ToString();
        }
    }
}