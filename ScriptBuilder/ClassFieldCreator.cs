
using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class ClassFieldCreator
    {
        public string AccessModifier;
        public string Type;
        public string Name;
        public int Spacing;
        public bool IsStatic;
        public string Value;
        public List<string> Attributes = new();
        public ClassCreator ClassCreator;

        private ClassFieldCreator(ClassCreator classCreator, string accessModifier, string type, string name)
        {
            ClassCreator = classCreator;
            AccessModifier = accessModifier;
            Type = type;
            Name = name;
            Spacing = classCreator.Spacing + 4;
        }

        public static ClassFieldCreator Create(ClassCreator classCreator, string accessModifier, string type, string name)
        {
            return new ClassFieldCreator(classCreator, accessModifier, type, name);
        }

        public ClassFieldCreator SetStatic()
        {
            IsStatic = true;
            return this;
        }

        public ClassFieldCreator AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }
        
        public ClassFieldCreator SetValue(string value)
        {
            Value = value;
            return this;
        }

        public ClassCreator End()
        {
            return ClassCreator;
        }

        public override string ToString()
        {
            var spacing = ScriptBuilder.GetSpacing(Spacing);
            var fieldBuilder = new StringBuilder();

            foreach (var attribute in Attributes)
            {
                fieldBuilder.AppendLine($"{spacing}[{attribute}]");
            }
            
            var staticPart = IsStatic || ClassCreator.IsStatic ? "static " : "";
            var value = string.IsNullOrEmpty(Value) ? "" : $" = {Value}";
            fieldBuilder.AppendLine($"{spacing}{AccessModifier} {staticPart}{Type} {Name}{value};");

            return fieldBuilder.ToString();
        }
    }
}