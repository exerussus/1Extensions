using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1Extensions.LoopFeature;
using Exerussus._1Extensions.Abstractions;
using Exerussus._1Extensions.SmallFeatures;
using Exerussus._1Extensions.GenericFeatures;
using Exerussus._1Extensions.Scripts.Extensions;
using Exerussus._1Extensions.Microservices.Core;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public static partial class Microservices
    {
        private static readonly Dictionary<Type, object> ChannelsSubs = new ();
        private static readonly Dictionary<Type, RegisteredService> RegisteredServices = new ();
        private static readonly GameShare GameShare = new();
        private static readonly Dictionary<Type, HashSet<Type>> PushersToChannels = new ();
        private static readonly Dictionary<Type, HashSet<Type>> ChannelsToPullers = new ();
        
        private static readonly object LockChannelsPullers = new();

        public static void UnregisterService<T>(T service) where T : IService
        {
            if (service == null) return;
            if (!RegisteredServices.TryPop(service.GetType(), out var registeredService)) return;

            foreach (var channelType in registeredService.PullChannels)
            {
                if (ChannelsToPullers.TryGetValue(channelType, out var pullers))
                {
                    pullers.Remove(registeredService.ServiceType);
                    if (pullers.Count == 0) ChannelsToPullers.Remove(channelType);
                }
            }

            PushersToChannels.Remove(registeredService.ServiceType);
            
            registeredService.Dispose();
        }

        public static void RegisterService(IService service)
        {
            if (service == null) return;
            var pullChannelTypes = GenericInterfaceInspector.GetGenericArgumentsFor(service.GetType(), typeof(IChannelPuller<>));
            var pushChannelTypes = GenericInterfaceInspector.GetGenericArgumentsFor(service.GetType(), typeof(IChannelPusher<>));
            
            var pullTypesArray = pullChannelTypes.Count == 0 ? null : pullChannelTypes.ToArray();
            var pushTypesArray = pushChannelTypes.Count == 0 ? null : pushChannelTypes.ToArray();
            
            var registeredService = new RegisteredService(service, pullTypesArray, pushTypesArray);
            
            GameShare.InjectSharedObjects(service);
            RegisteredServices.Add(registeredService.ServiceType, registeredService);

            if (service is IServiceInspector inspector)
            {
                inspector.GameShare = GameShare;
                inspector.RegisteredServices = RegisteredServices;
                inspector.PushersToChannels = PushersToChannels;
                inspector.ChannelsToPullers = ChannelsToPullers;
                inspector.ChannelsSubs = ChannelsSubs;
                inspector.LockChannelsPullers = LockChannelsPullers;
            }

            if (pullChannelTypes.Count > 0)
            {
                ChannelPullerRegistrar.RegisterAllPullers(service);

                foreach (var channelType in pullChannelTypes)
                {
                    if (!ChannelsToPullers.TryGetValue(channelType, out var pullers))
                    {
                        pullers = new HashSet<Type>();
                        ChannelsToPullers.Add(channelType, pullers);
                    }

                    pullers.Add(registeredService.ServiceType);
                }
            }

            if (pushChannelTypes.Count > 0)
            {
                if (!PushersToChannels.TryGetValue(registeredService.ServiceType, out var channels))
                {
                    channels = new HashSet<Type>();
                    PushersToChannels.Add(registeredService.ServiceType, channels);
                }
                
                foreach (var channelType in pushChannelTypes) channels.Add(channelType);
            }
            
            TryRegisterUpdating(registeredService);
        }
        
        public static void RegisterTool<T>(T tool)
        {
            GameShare.AddSharedObject(tool);
        }

        internal static void RegisterPuller<TChannel>(object instance, Func<TChannel, UniTask> puller) where TChannel : IChannel
        {
            Debug.Log($"Microservices | RegisterPuller : {typeof(TChannel).Name} in {instance.GetType().Name}.");
            var registeredService = RegisteredServices[instance.GetType()];
            
            AddToPool<TChannel>(registeredService, puller);
        }
        
        private static void AddToPool<T>(RegisteredService registeredService, Func<T, UniTask> pull) where T : IChannel
        {
            lock (LockChannelsPullers)
            {
                var type = typeof(T);
                Func<T, UniTask> subs;

                if (!ChannelsSubs.TryGetValue(type, out var subObject))
                {
                    subs = pull;
                    ChannelsSubs.Add(type, subs);
                }
                else
                {
                    subs = subObject as Func<T, UniTask>;
                    subs += pull;
                    ChannelsSubs[type] = subs;
                }

                registeredService.DisposeActions += () =>
                {
                    if (ChannelsSubs == null) return;
                    if (ChannelsSubs.Count == 0) return;
                    if (!ChannelsSubs.TryGetValue(type, out var subObj)) return;
                    if (subObj is not Func<T, UniTask> subscribes) return;
                    subscribes -= pull;
                    lock (LockChannelsPullers)
                    {
                        ChannelsSubs[type] = subscribes;
                    }
                };
            }
        }

        private static void TryRegisterUpdating(RegisteredService registeredService)
        {
            if (registeredService.Service is IUpdatable updatableService and not MonoBehaviour)
            {
                ExerussusLoopHelper.OnUpdate -= updatableService.Update;
                ExerussusLoopHelper.OnUpdate += updatableService.Update;
                
                registeredService.DisposeActions += () => ExerussusLoopHelper.OnUpdate -= updatableService.Update;
            }
        }

        public static async UniTask PushBroadcast<T>(T channel) where T : IChannel
        {
            var type = typeof(T);
            
            if (!ChannelsSubs.TryGetValue(type, out var subObj)) return;
            if (subObj is not Func<T, UniTask> subs) return;
            await subs(channel);
        }

        public static void UnregisterAll()
        {
            foreach (var registeredService in RegisteredServices.Values) registeredService.Dispose();
            
            RegisteredServices.Clear();
            ChannelsSubs.Clear();
        }
    }
}
