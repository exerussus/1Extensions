using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class StructFieldCreator
    {
        public string AccessModifier;
        public int Spacing;
        public string Type;
        public string Name;
        public bool IsStatic;
        public string Value;
        public List<string> Attributes = new();
        public StructCreator StructCreator;
        
        private StructFieldCreator(StructCreator classCreator, string accessModifier, string type, string name)
        {
            StructCreator = classCreator;
            AccessModifier = accessModifier;
            Type = type;
            Name = name;
            Spacing = classCreator.Spacing + 4;
        }

        public static StructFieldCreator Create(StructCreator classCreator, string accessModifier, string type, string name)
        {
            return new StructFieldCreator(classCreator, accessModifier, type, name);
        }

        public StructFieldCreator SetStatic()
        {
            IsStatic = true;
            return this;
        }

        public StructFieldCreator AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }
        
        public StructFieldCreator SetValue(string value)
        {
            Value = value;
            return this;
        }

        public StructCreator End()
        {
            return StructCreator;
        }

        public override string ToString()
        {
            var spacing = ScriptBuilder.GetSpacing(Spacing);
            var fieldBuilder = new StringBuilder();

            foreach (var attribute in Attributes)
            {
                fieldBuilder.AppendLine($"{spacing}[{attribute}]");
            }
            
            var staticPart = IsStatic ? "static " : "";
            var value = string.IsNullOrEmpty(Value) ? "" : $" = {Value}";
            fieldBuilder.AppendLine($"{spacing}{AccessModifier} {staticPart}{Type} {Name}{value};");

            return fieldBuilder.ToString();
        }
    }
}