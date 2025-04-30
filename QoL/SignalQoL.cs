using System;
using System.Threading.Tasks;
using Exerussus._1Extensions.SignalSystem;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class SignalQoL
    {
        public static Signal Instance;
        private static bool _isInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            Instance = new();
        }

        public static void RegistryRaise<T>(T data) where T : struct => Instance.RegistryRaise(data);

        public static void RegistryRaise<T>() where T : struct => Instance.RegistryRaise<T>();

        public static async Task<TContext> RegistryRaiseAsync<TData, TContext>(TData data, int delay = 100, int timeout = 10000)
            where TData : struct, IAsyncSignal<TContext>
            where TContext : SignalContext, new() =>
            await Instance.RegistryRaiseAsync<TData, TContext>(data, delay, timeout);

        public static async Task<ResultContext> RegistryRaiseAsync<TData>(TData data, int delay = 100, int timeout = 10000)
            where TData : struct, IAsyncSignal<ResultContext> =>
            await Instance.RegistryRaiseAsync(data, delay, timeout);

        public static async Task<TContext> RegistryRaiseAsync<TData, TContext>(int delay = 100, int timeout = 10000)
            where TData : struct, IAsyncSignal<TContext>
            where TContext : SignalContext, new() =>
            await Instance.RegistryRaiseAsync<TData, TContext>(delay, timeout);

        public static async Task<ResultContext> RegistryRaiseAsync<TData>(int delay = 100, int timeout = 10000) where TData : struct, IAsyncSignal<ResultContext> => await Instance.RegistryRaiseAsync<TData>(delay, timeout);

        public static void RegistryRaise<T>(ref T data) where T : struct => Instance.RegistryRaise(ref data);

        public static void Subscribe<T>(Action<T> action) where T : struct => Instance.Subscribe(action);

        public static void SubscribeShot<T>(Action<T> action) where T : struct => Instance.SubscribeShot(action);

        public static void SubscribeShot<T>(string id, Action<T> action) where T : struct => Instance.SubscribeShot(id, action);

        public static void Unsubscribe<T>(Action<T> action) where T : struct => Instance.Unsubscribe(action);
    }
}