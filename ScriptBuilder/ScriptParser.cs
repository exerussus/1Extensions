
using System;
using System.Linq;
using System.Reflection;

namespace Exerussus._1Extensions.ScriptBuilding
{
    public class ScriptParser
    {
        public static ScriptBuilder GenerateScriptBuilderFromType(Type type)
        {
            // Создаем ScriptBuilder и добавляем пространство имен
            var scriptBuilder = ScriptBuilder.Create(type.Namespace);

            // Добавляем "using" для всех необходимых пространств имен
            var usings = type.Assembly.GetReferencedAssemblies()
                .Select(a => a.Name).Distinct();
            foreach (var u in usings)
            {
                scriptBuilder.AddUsing(u);
            }

            // Добавляем основной класс и все его элементы
            ProcessType(type, scriptBuilder);

            return scriptBuilder;
        }

        private static void ProcessType(Type type, ScriptBuilder scriptBuilder)
        {
            if (type.IsEnum)
            {
                // Обработка перечислений
                var enumCreator = scriptBuilder.AddEnum(type.Name);
                foreach (var value in Enum.GetNames(type))
                {
                    enumCreator.AddValue(value);
                }
            }
            else
            {
                // Создаем класс
                var classCreator = scriptBuilder.AddClass(type.Name);

                // Добавляем интерфейсы или наследования
                foreach (var iface in type.GetInterfaces())
                {
                    classCreator.AddInheritance(iface.Name);
                }

                // Обрабатываем поля
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                     BindingFlags.Instance | BindingFlags.Static))
                {
                    var fieldCreator =
                        classCreator.AddField(GetAccessModifier(field), field.FieldType.Name, field.Name);
                    if (field.IsStatic)
                    {
                        fieldCreator.SetStatic();
                    }
                }

                // Обрабатываем методы
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                       BindingFlags.Instance | BindingFlags.Static))
                {
                    // Пропускаем специальные методы
                    if (method.IsSpecialName) continue;

                    var methodCreator = classCreator.AddMethod(method.Name);
                    if (method.IsStatic)
                    {
                        methodCreator.SetStatic();
                    }

                    // Добавляем параметры
                    foreach (var param in method.GetParameters())
                    {
                        methodCreator.AddParam(param.ParameterType.Name, param.Name);
                    }

                    // Добавляем пустую строку в методе для тела (опционально можно добавить примерный текст)
                    methodCreator.AddLine("// Method implementation");
                }

                // Обрабатываем вложенные типы (классы и перечисления)
                foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
                {
                    ProcessType(nestedType, scriptBuilder);
                }
            }
        }

        private static string GetAccessModifier(FieldInfo field)
        {
            if (field.IsPublic) return "public";
            if (field.IsPrivate) return "private";
            if (field.IsFamily) return "protected";
            if (field.IsAssembly) return "internal";
            return "private";
        }

        private static string GetAccessModifier(MethodInfo method)
        {
            if (method.IsPublic) return "public";
            if (method.IsPrivate) return "private";
            if (method.IsFamily) return "protected";
            if (method.IsAssembly) return "internal";
            return "private";
        }
    }
}
