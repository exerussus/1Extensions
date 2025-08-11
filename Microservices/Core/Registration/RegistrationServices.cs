
using Exerussus._1Extensions.Scripts.Extensions;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public static partial class Microservices
    {
        public static void UnregisterService<T>(T service) where T : IService
        {
            if (!RegisteredServices.TryPop(typeof(T), out var registeredService)) return;
            
            registeredService.Dispose();
        }

        public static void RegisterService(IService service)
        {
            var registeredService = new RegisteredService(service, null);
            RegisteredServices.Add(registeredService.ServiceType, registeredService);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1>(IService<T1> service) where T1 : IChannel
        {
            var typeChannel1 = typeof(T1);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }

        public static void RegisterService<T1, T2>(IService<T1, T2> service) 
            where T1 : IChannel
            where T2 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            
            var registeredService = new RegisteredService(service, new [] 
            { 
                typeChannel1,
                typeChannel2 
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }

        public static void RegisterService<T1, T2, T3>(IService<T1, T2, T3> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1, T2, T3, T4>(IService<T1, T2, T3, T4> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            var typeChannel4 = typeof(T4);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3,
                typeChannel4
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            AddToPool<T4>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5>(IService<T1, T2, T3, T4, T5> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            var typeChannel4 = typeof(T4);
            var typeChannel5 = typeof(T5);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3,
                typeChannel4,
                typeChannel5
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            AddToPool<T4>(registeredService, service.PullBroadcast);
            AddToPool<T5>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6>(IService<T1, T2, T3, T4, T5, T6> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            var typeChannel4 = typeof(T4);
            var typeChannel5 = typeof(T5);
            var typeChannel6 = typeof(T6);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3,
                typeChannel4,
                typeChannel5,
                typeChannel6
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            AddToPool<T4>(registeredService, service.PullBroadcast);
            AddToPool<T5>(registeredService, service.PullBroadcast);
            AddToPool<T6>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7>(IService<T1, T2, T3, T4, T5, T6, T7> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
            where T7 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            var typeChannel4 = typeof(T4);
            var typeChannel5 = typeof(T5);
            var typeChannel6 = typeof(T6);
            var typeChannel7 = typeof(T7);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3,
                typeChannel4,
                typeChannel5,
                typeChannel6,
                typeChannel7
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            AddToPool<T4>(registeredService, service.PullBroadcast);
            AddToPool<T5>(registeredService, service.PullBroadcast);
            AddToPool<T6>(registeredService, service.PullBroadcast);
            AddToPool<T7>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7, T8>(IService<T1, T2, T3, T4, T5, T6, T7, T8> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
            where T7 : IChannel
            where T8 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            var typeChannel4 = typeof(T4);
            var typeChannel5 = typeof(T5);
            var typeChannel6 = typeof(T6);
            var typeChannel7 = typeof(T7);
            var typeChannel8 = typeof(T8);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3,
                typeChannel4,
                typeChannel5,
                typeChannel6,
                typeChannel7,
                typeChannel8
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            AddToPool<T4>(registeredService, service.PullBroadcast);
            AddToPool<T5>(registeredService, service.PullBroadcast);
            AddToPool<T6>(registeredService, service.PullBroadcast);
            AddToPool<T7>(registeredService, service.PullBroadcast);
            AddToPool<T8>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IService<T1, T2, T3, T4, T5, T6, T7, T8, T9> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
            where T7 : IChannel
            where T8 : IChannel
            where T9 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            var typeChannel4 = typeof(T4);
            var typeChannel5 = typeof(T5);
            var typeChannel6 = typeof(T6);
            var typeChannel7 = typeof(T7);
            var typeChannel8 = typeof(T8);
            var typeChannel9 = typeof(T9);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3,
                typeChannel4,
                typeChannel5,
                typeChannel6,
                typeChannel7,
                typeChannel8,
                typeChannel9
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            AddToPool<T4>(registeredService, service.PullBroadcast);
            AddToPool<T5>(registeredService, service.PullBroadcast);
            AddToPool<T6>(registeredService, service.PullBroadcast);
            AddToPool<T7>(registeredService, service.PullBroadcast);
            AddToPool<T8>(registeredService, service.PullBroadcast);
            AddToPool<T9>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> service) 
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
            where T7 : IChannel
            where T8 : IChannel
            where T9 : IChannel
            where T10 : IChannel
        {
            var typeChannel1 = typeof(T1);
            var typeChannel2 = typeof(T2);
            var typeChannel3 = typeof(T3);
            var typeChannel4 = typeof(T4);
            var typeChannel5 = typeof(T5);
            var typeChannel6 = typeof(T6);
            var typeChannel7 = typeof(T7);
            var typeChannel8 = typeof(T8);
            var typeChannel9 = typeof(T9);
            var typeChannel10 = typeof(T10);
            
            var registeredService = new RegisteredService(service, new []
            {
                typeChannel1,
                typeChannel2,
                typeChannel3,
                typeChannel4,
                typeChannel5,
                typeChannel6,
                typeChannel7,
                typeChannel8,
                typeChannel9,
                typeChannel10
            });
            
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            AddToPool<T1>(registeredService, service.PullBroadcast);
            AddToPool<T2>(registeredService, service.PullBroadcast);
            AddToPool<T3>(registeredService, service.PullBroadcast);
            AddToPool<T4>(registeredService, service.PullBroadcast);
            AddToPool<T5>(registeredService, service.PullBroadcast);
            AddToPool<T6>(registeredService, service.PullBroadcast);
            AddToPool<T7>(registeredService, service.PullBroadcast);
            AddToPool<T8>(registeredService, service.PullBroadcast);
            AddToPool<T9>(registeredService, service.PullBroadcast);
            AddToPool<T10>(registeredService, service.PullBroadcast);
            TryRegisterUpdating(registeredService);
        }
    }
}