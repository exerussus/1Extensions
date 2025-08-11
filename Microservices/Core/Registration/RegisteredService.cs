using System;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public class RegisteredService : IDisposable
    {
        public RegisteredService(IService service, Type[] subChannels)
        {
            Service = service;
            SubChannels = subChannels ?? Array.Empty<Type>();
            ServiceType = service.GetType();
        }

        public readonly Type ServiceType;
        public readonly IService Service;
        public readonly Type[] SubChannels;
        public event Action DisposeActions;
        
        public void Dispose()
        {
            DisposeActions?.Invoke();
        }
    }
}