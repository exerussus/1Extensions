using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class EnumCreator
    {
        private string _name;
        private List<string> _values = new();
        private List<string> _attributes = new();
        private ScriptBuilder _scriptCreator;

        public string Name => _name;
        public List<string> Values => _values;

        private EnumCreator(ScriptBuilder scriptCreator, string name)
        {
            _scriptCreator = scriptCreator;
            _name = name;
        }

        public static EnumCreator Create(ScriptBuilder scriptCreator, string name)
        {
            return new EnumCreator(scriptCreator, name);
        }

        public EnumCreator AddAttribute(string attribute)
        {
            _attributes.Add(attribute);
            return this;
        }

        public EnumCreator AddValue(string value)
        {
            _values.Add(value);
            return this;
        }

        public ScriptBuilder End()
        {
            return _scriptCreator;
        }

        public override string ToString()
        {
            var enumBuilder = new StringBuilder();

            foreach (var attribute in _attributes)
            {
                enumBuilder.AppendLine($"    [{attribute}]");
            }

            enumBuilder.AppendLine($"    public enum {_name}");
            enumBuilder.AppendLine("    {");

            for (int i = 0; i < _values.Count; i++)
            {
                enumBuilder.AppendLine(i == _values.Count - 1
                    ? $"        {_values[i]}"
                    : $"        {_values[i]},");
            }

            enumBuilder.AppendLine("    }");

            return enumBuilder.ToString();
        }
    }
}