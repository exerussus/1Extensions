
using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class FieldCreator
    {
        private string _accessModifier;
        private string _type;
        private string _name;
        private bool _isStatic;
        private string _value;
        private List<string> _attributes = new();
        private ClassCreator _classCreator;

        private FieldCreator(ClassCreator classCreator, string accessModifier, string type, string name)
        {
            _classCreator = classCreator;
            _accessModifier = accessModifier;
            _type = type;
            _name = name;
        }

        public static FieldCreator Create(ClassCreator classCreator, string accessModifier, string type, string name)
        {
            return new FieldCreator(classCreator, accessModifier, type, name);
        }

        public FieldCreator SetStatic()
        {
            _isStatic = true;
            return this;
        }

        public FieldCreator AddAttribute(string attribute)
        {
            _attributes.Add(attribute);
            return this;
        }
        
        public FieldCreator SetValue(string value)
        {
            _value = value;
            return this;
        }

        public ClassCreator End()
        {
            return _classCreator;
        }

        public override string ToString()
        {
            var fieldBuilder = new StringBuilder();

            foreach (var attribute in _attributes)
            {
                fieldBuilder.AppendLine($"        [{attribute}]");
            }
            
            var staticPart = _isStatic ? "static " : "";
            var value = string.IsNullOrEmpty(_value) ? "" : $" = {_value}";
            fieldBuilder.AppendLine($"        {_accessModifier} {staticPart}{_type} {_name}{value};");

            return fieldBuilder.ToString();
        }
    }
}