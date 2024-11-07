using System.Collections.Generic;
using System.Text;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class ScriptBuilder
    {
        private string _nameSpace;
        private List<ClassCreator> _classes = new();
        private List<string> _usings = new();

        private ScriptBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
        }

        public static ScriptBuilder Create(string name)
        {
            return new ScriptBuilder(name);
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

            scriptBuilder.AppendLine("}");

            return scriptBuilder.ToString();
        }
    }
}