using System;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public class RegisteredService : IDisposable
    {
        public RegisteredService(IService service, Type[] pullChannels, Type[] pushChannels)
        {
            Service = service;
            PullChannels = pullChannels ?? Array.Empty<Type>();
            PushChannels = pushChannels ?? Array.Empty<Type>();
            ServiceType = service.GetType();
        }

        public readonly Type ServiceType;
        public readonly IService Service;
        public readonly Type[] PullChannels;
        public readonly Type[] PushChannels;
        public event Action DisposeActions;
        
        public void Dispose()
        {
            DisposeActions?.Invoke();
        }
    }
}