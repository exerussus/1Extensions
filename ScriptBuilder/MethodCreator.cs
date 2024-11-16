using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class MethodCreator
    {
        public string Name;
        public bool IsStatic;
        public int Spacing;
        public ClassCreator ClassCreator;
        public List<string> Lines = new();
        public List<string> Params = new();
        public List<string> Attributes = new();

        private MethodCreator(ClassCreator classCreator, string name)
        {
            ClassCreator = classCreator;
            Spacing = classCreator.Spacing + 4;
            Name = name;
        }

        public static MethodCreator Create(ClassCreator classCreator, string name)
        {
            return new MethodCreator(classCreator, name);
        }

        public MethodCreator AddParam(string type, string name)
        {
            Params.Add($"{type} {name}");
            return this;
        }

        public MethodCreator SetStatic()
        {
            IsStatic = true;
            return this;
        }

        public MethodCreator AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public MethodCreator AddLine(string line)
        {
            Lines.Add(line);
            return this;
        }

        public MethodCreator AddSpace()
        {
            Lines.Add("");
            return this;
        }

        public ClassCreator End()
        {
            return ClassCreator;
        }

        public override string ToString()
        {
            var spacing = ScriptBuilder.GetSpacing(Spacing);
            var lineSpacing = ScriptBuilder.GetSpacing(Spacing + 4);
            var methodBuilder = new StringBuilder();

            foreach (var attribute in Attributes)
            {
                methodBuilder.AppendLine($"{spacing}[{attribute}]");
            }

            var staticPart = IsStatic ? "static " : "";
            var paramsList = Params.Count > 0 ? string.Join(", ", Params) : "";
            methodBuilder.AppendLine($"{spacing}public {staticPart}void {Name}({paramsList})");
            methodBuilder.AppendLine(spacing + "{");

            foreach (var line in Lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    methodBuilder.AppendLine();
                }
                else
                {
                    methodBuilder.AppendLine($"{lineSpacing}{line}");
                }
            }

            methodBuilder.AppendLine(spacing + "}");

            return methodBuilder.ToString();
        }
    }
}