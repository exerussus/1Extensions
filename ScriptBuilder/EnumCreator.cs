using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class EnumCreator
    {
        public string Name;
        public int Spacing;
        public List<string> Values = new();
        public List<string> Attributes = new();
        public ScriptBuilder ScriptCreator;

        private EnumCreator(ScriptBuilder scriptCreator, string name)
        {
            ScriptCreator = scriptCreator;
            Name = name;
            Spacing = 4;
        }

        public static EnumCreator Create(ScriptBuilder scriptCreator, string name)
        {
            return new EnumCreator(scriptCreator, name);
        }

        public EnumCreator AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public EnumCreator AddValue(string value)
        {
            Values.Add(value);
            return this;
        }

        public ScriptBuilder End()
        {
            return ScriptCreator;
        }

        public override string ToString()
        {
            var spacing = ScriptBuilder.GetSpacing(Spacing);
            var elementSpacing = ScriptBuilder.GetSpacing(Spacing + 4);
            var enumBuilder = new StringBuilder();

            foreach (var attribute in Attributes)
            {
                enumBuilder.AppendLine($"{spacing}[{attribute}]");
            }

            enumBuilder.AppendLine($"{spacing}public enum {Name}");
            enumBuilder.AppendLine(spacing + "{");

            for (int i = 0; i < Values.Count; i++) enumBuilder.AppendLine($"{elementSpacing}{Values[i]},");
            
            enumBuilder.AppendLine(spacing + "}");

            return enumBuilder.ToString();
        }
    }
}