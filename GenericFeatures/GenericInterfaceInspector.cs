
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;


namespace Exerussus._1Extensions.GenericFeatures
{
    public static class GenericInterfaceInspector
    {
        /// <summary>
        /// Вернёт все generic-аргументы для конкретного открытого интерфейса (например, IFoo&lt;&gt;).
        /// Если тип не реализует интерфейс — вернёт пустой массив.
        /// </summary>
        [Preserve]
        public static List<Type> GetGenericArgumentsFor(Type inspectedType, Type openInterface)
        {
            if (inspectedType == null || openInterface == null) return new List<Type>();

            var result = new List<Type>();

            var interfaces = inspectedType.GetInterfaces();
            for (var i = interfaces.Length - 1; i >= 0; i--)
            {
                var itf = interfaces[i];
                if (!itf.IsGenericType) continue;

                if (itf.GetGenericTypeDefinition() == openInterface)
                {
                    result.AddRange(itf.GetGenericArguments());
                }
            }

            return result;
        }
    }
}