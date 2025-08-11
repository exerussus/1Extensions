using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Exerussus._1Extensions.Abstractions;
using Exerussus._1Extensions.LoopFeature;

namespace Exerussus._1Extensions.MicroserviceFeature
{
    public static partial class Microservices
    {
        private static readonly Dictionary<Type, object> ChannelsSubs = new ();
        private static readonly Dictionary<Type, RegisteredService> RegisteredServices = new ();
        private static readonly object LockChannelsSubs = new();
        
        private static void AddToPool<T>(RegisteredService registeredService, Func<T, UniTask> pull) where T : IChannel
        {
            lock (LockChannelsSubs)
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
                    lock (LockChannelsSubs)
                    {
                        ChannelsSubs[type] = subscribes;
                    }
                };
            }
        }

        private static void TryRegisterUpdating(RegisteredService registeredService)
        {
            if (registeredService.Service is IUpdatable updatableService)
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
