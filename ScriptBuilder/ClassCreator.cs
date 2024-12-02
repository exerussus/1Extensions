using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class ClassCreator
    {
        public string Name;
        public bool IsStatic;
        public int Spacing;
        public List<string> Inheritances = new();
        public List<string> Attributes = new();
        public List<ClassFieldCreator> Fields = new();
        public List<ClassPropertyCreator> Properties = new();
        public List<MethodCreator> Methods = new();
        public List<ClassCreator> SubClasses = new();
        public ScriptBuilder ScriptBuilder;
        public ClassCreator ParentClass;

        private ClassCreator(ScriptBuilder scriptBuilder, string name)
        {
            Spacing = 4;
            ScriptBuilder = scriptBuilder;
            Name = name;
        }
        
        private ClassCreator(ClassCreator classCreator, string name)
        {
            Spacing = classCreator.Spacing + 4;
            ParentClass = classCreator;
            ScriptBuilder = classCreator.ScriptBuilder;
            Name = name;
        }

        public static ClassCreator Create(ScriptBuilder scriptBuilder, string name)
        {
            var newSubClass = new ClassCreator(scriptBuilder, name);
            return newSubClass;
        }

        public ClassCreator AddSubClass(string name)
        {
            var newSubClass = new ClassCreator(this, name);
            SubClasses.Add(newSubClass);
            return newSubClass;
        }

        public ClassCreator SetSpacing(int value)
        {
            Spacing = value;
            return this;
        }
        
        public ClassCreator AddInheritance(string inheritance)
        {
            Inheritances.Add(inheritance);
            return this;
        }

        public ClassCreator SetStatic()
        {
            IsStatic = true;
            return this;
        }

        public ClassCreator AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }

        public ClassFieldCreator AddField(string accessModifier, string type, string name)
        {
            var field = ClassFieldCreator.Create(this, accessModifier, type, name);
            Fields.Add(field);
            return field;
        }

        public object AddProperty(string accessModifier, string type, string name)
        {
            var property = ClassPropertyCreator.Create(this, accessModifier, type, name);
            Properties.Add(property);
            return property;
        }

        public MethodCreator AddMethod(string name)
        {
            var method = MethodCreator.Create(this, name);
            Methods.Add(method);
            return method;
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
            
            var staticPart = IsStatic ? "static " : "";
            var inheritancePart = Inheritances.Count > 0 ? " : " + string.Join(", ", Inheritances) : "";
            stringBuilder.AppendLine($"{spacing}public {staticPart}class {Name}{inheritancePart}");
            stringBuilder.AppendLine($"{spacing}"+"{");

            foreach (var field in Fields) stringBuilder.Append(field);

            if (Fields.Count > 0 && Methods.Count > 0) stringBuilder.AppendLine();

            for (var index = 0; index < Methods.Count; index++)
            {
                var method = Methods[index];
                if (index > 0) stringBuilder.AppendLine();
                stringBuilder.Append(method);
            }
            
            for (var index = 0; index < SubClasses.Count; index++)
            {
                var @class = SubClasses[index];
                if (index > 0) stringBuilder.AppendLine();
                stringBuilder.Append(@class);
            }

            stringBuilder.AppendLine(spacing + "}");

            return stringBuilder.ToString();
        }
    }
}