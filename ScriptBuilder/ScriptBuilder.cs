using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class ScriptBuilder
    {
        private string _nameSpace;
        private List<ClassCreator> _classes = new();
        private List<StructCreator> _structs = new();
        private List<EnumCreator> _enums = new();
        private List<string> _usings = new();

        private ScriptBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
        }

        public static ScriptBuilder Create(string name)
        {
            return new ScriptBuilder(name);
        }

        public static string GetSpacing(int spaceValue)
        {
            var spacing = "";
            for (int i = 0; i < spaceValue; i++) spacing += " ";
            return spacing;
        }
        
        public ScriptBuilder AddUsing(string reference)
        {
            _usings.Add($"using {reference};");
            return this;
        }

        public ClassCreator AddClass(string name)
        {
            var classCreator = ClassCreator.Create(this, name);
            _classes.Add(classCreator);
            return classCreator;
        }

        public StructCreator AddStruct(string name)
        {
            var structCreator = StructCreator.Create(this, name);
            _structs.Add(structCreator);
            return structCreator;
        }

        public EnumCreator AddEnum(string name)
        {
            var enumCreator = EnumCreator.Create(this, name);
            _enums.Add(enumCreator);
            return enumCreator;
        }

        public override string ToString()
        {
            return End();
        }

        public string End()
        {
            var scriptBuilder = new StringBuilder();

            foreach (var use in _usings)
            {
                scriptBuilder.AppendLine(use);
            }

            scriptBuilder.AppendLine();

            scriptBuilder.AppendLine($"namespace {_nameSpace}");
            scriptBuilder.AppendLine("{");

            for (var index = 0; index < _classes.Count; index++)
            {
                var classCreator = _classes[index];
                if (index > 0) scriptBuilder.AppendLine("");
                scriptBuilder.Append(classCreator);
            }
            if (_classes.Count > 0) scriptBuilder.AppendLine("");
            
            for (var index = 0; index < _structs.Count; index++)
            {
                var structCreator = _structs[index];
                if (index > 0) scriptBuilder.AppendLine("");
                scriptBuilder.Append(structCreator);
            }
            
            if (_structs.Count > 0) scriptBuilder.AppendLine("");
            
            for (var index = 0; index < _enums.Count; index++)
            {
                var enumCreator = _enums[index];
                if (index > 0) scriptBuilder.AppendLine("");
                scriptBuilder.Append(enumCreator);
            }

            scriptBuilder.AppendLine("}");

            return scriptBuilder.ToString();
        }
    }
}