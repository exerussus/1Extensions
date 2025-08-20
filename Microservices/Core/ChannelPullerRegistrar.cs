using System;
using System.Linq;
using System.Reflection;
using Exerussus._1Extensions.MicroserviceFeature;


namespace Exerussus._1Extensions.Microservices.Core
{
    public static class ChannelPullerRegistrar
    {
        public static void RegisterAllPullers(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var type = instance.GetType();

            var pullerInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IChannelPuller<>))
                .Reverse();

            foreach (var itf in pullerInterfaces)
            {
                var method = itf.GetMethod("RegisterPuller", BindingFlags.Instance | BindingFlags.NonPublic);
                if (method == null) throw new InvalidOperationException($"У интерфейса {itf} нет RegisterPuller");
                method.Invoke(instance, null);
            }
        }
    }
}