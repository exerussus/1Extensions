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
    }
}