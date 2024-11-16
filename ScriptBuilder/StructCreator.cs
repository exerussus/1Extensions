using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class StructCreator
    {
        public string Name;
        public int Spacing;
        public List<string> Inheritances = new();
        public List<string> Attributes = new();
        public List<StructFieldCreator> Fields = new();
        public List<MethodCreator> Methods = new();
        public ScriptBuilder ScriptBuilder;
        public ClassCreator ParentClass;

        private StructCreator(ScriptBuilder scriptBuilder, string name)
        {
            Spacing = 4;
            ScriptBuilder = scriptBuilder;
            Name = name;
        }

        private StructCreator(ClassCreator classCreator, string name)
        {
            Spacing = classCreator.Spacing + 4;
            ParentClass = classCreator;
            ScriptBuilder = classCreator.ScriptBuilder;
            Name = name;
        }

        public static StructCreator Create(ScriptBuilder scriptBuilder, string name)
        {
            return new StructCreator(scriptBuilder, name);
        }

        public StructCreator SetSpacing(int value)
        {
            Spacing = value;
            return this;
        }
        
        public StructCreator AddInheritance(string inheritance)
        {
            Inheritances.Add(inheritance);
            return this;
        }

        public StructCreator AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public StructFieldCreator AddField(string accessModifier, string type, string name)
        {
            var field = StructFieldCreator.Create(this, accessModifier, type, name);
            Fields.Add(field);
            return field;
        }

        public ClassCreator EndSubClass()
        {
            return ParentClass;
        }

        public ScriptBuilder End()
        {
            return ScriptBuilder;
        }
        public override string ToString()
        {
            var spacing = ScriptBuilder.GetSpacing(Spacing);
            
            var stringBuilder = new StringBuilder();

            foreach (var attribute in Attributes) stringBuilder.AppendLine($"{spacing}[{attribute}]");
            
            var inheritancePart = Inheritances.Count > 0 ? " : " + string.Join(", ", Inheritances) : "";
            stringBuilder.AppendLine($"{spacing}public struct {Name}{inheritancePart}");
            stringBuilder.AppendLine($"{spacing}"+"{");

            foreach (var field in Fields) stringBuilder.Append(field);

            if (Fields.Count > 0 && Methods.Count > 0) stringBuilder.AppendLine();

            for (var index = 0; index < Methods.Count; index++)
            {
                var method = Methods[index];
                if (index > 0) stringBuilder.AppendLine();
                stringBuilder.Append(method);
            }

            stringBuilder.AppendLine(spacing + "}");

            return stringBuilder.ToString();
        }
    }
}