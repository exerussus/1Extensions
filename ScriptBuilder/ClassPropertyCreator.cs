using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class ClassPropertyCreator
    {
        public string AccessModifier;
        public string Type;
        public string Name;
        public int Spacing;
        public bool IsStatic;
        public List<string> Attributes = new();
        public ClassCreator ClassCreator;

        private ClassPropertyCreator(ClassCreator classCreator, string accessModifier, string type, string name)
        {
            ClassCreator = classCreator;
            AccessModifier = accessModifier;
            Type = type;
            Name = name;
            Spacing = classCreator.Spacing + 4;
        }

        public static ClassPropertyCreator Create(ClassCreator classCreator, string accessModifier, string type, string name)
        {
            return new ClassPropertyCreator(classCreator, accessModifier, type, name);
        }

        public ClassPropertyCreator SetStatic()
        {
            IsStatic = true;
            return this;
        }

        public ClassPropertyCreator AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public ClassCreator End()
        {
            return ClassCreator;
        }

        public override string ToString()
        {
            var spacing = ScriptBuilder.GetSpacing(Spacing);
            var propertyBuilder = new StringBuilder();

            foreach (var attribute in Attributes)
            {
                propertyBuilder.AppendLine($"{spacing}[{attribute}]");
            }

            var staticPart = IsStatic || ClassCreator.IsStatic ? "static " : "";
            propertyBuilder.AppendLine($"{spacing}{AccessModifier} {staticPart}{Type} {Name} {{ get; set; }}");

            return propertyBuilder.ToString();
        }
    }
}