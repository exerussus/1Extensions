namespace Exerussus._1Extensions.MicroserviceFeature
{
    public static class RegistrationExtensions
    {
        public static void UnregisterService(this IService service)
        {
            Microservices.UnregisterService(service);
        }
        
        public static void RegisterService(this IService service)
        {
            Microservices.RegisterService(service);
        }

        public static void RegisterService<T1>(this IService<T1> service)
            where T1 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2>(this IService<T1, T2> service)
            where T1 : IChannel
            where T2 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3>(this IService<T1, T2, T3> service)
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3, T4>(this IService<T1, T2, T3, T4> service)
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5>(this IService<T1, T2, T3, T4, T5> service)
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6>(this IService<T1, T2, T3, T4, T5, T6> service)
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7>(this IService<T1, T2, T3, T4, T5, T6, T7> service)
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
            where T7 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7, T8>(this IService<T1, T2, T3, T4, T5, T6, T7, T8> service)
            where T1 : IChannel
            where T2 : IChannel
            where T3 : IChannel
            where T4 : IChannel
            where T5 : IChannel
            where T6 : IChannel
            where T7 : IChannel
            where T8 : IChannel
        {
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IService<T1, T2, T3, T4, T5, T6, T7, T8, T9> service)
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
            Microservices.RegisterService(service);
        }
        
        public static void RegisterService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> service)
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
            Microservices.RegisterService(service);
        }
    }
}