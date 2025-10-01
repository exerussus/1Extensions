using System;
using System.Runtime.CompilerServices;
using Exerussus._1Extensions.SignalSystem;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class SignalQoL
    {
        public static Signal Instance;
        private static bool _isInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            Instance = new();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegistryRaise<T>(T data) where T : struct => Instance.RegistryRaise(data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegistryRaise<T>() where T : struct => Instance.RegistryRaise<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegistryRaise<T>(ref T data) where T : struct => Instance.RegistryRaise(ref data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T>(Action<T> action) where T : struct => Instance.Subscribe(action);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubscribeShot<T>(Action<T> action) where T : struct => Instance.SubscribeShot(action);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubscribeShot<T>(string id, Action<T> action) where T : struct => Instance.SubscribeShot(id, action);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Unsubscribe<T>(Action<T> action) where T : struct => Instance.Unsubscribe(action);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubscribeCancelable<TData, TContext>(Action<TData> action) where TData : struct, ISignalWithContext<TContext>
            where TContext : SignalContext, ICancelableSignal => Instance.SubscribeCancelable<TData, TContext>(action);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnsubscribeCancelable<TData, TContext>(Action<TData> action) where TData : struct, ISignalWithContext<TContext> 
            where TContext : SignalContext, ICancelableSignal => Instance.UnsubscribeCancelable<TData, TContext>(action);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RaiseCancelableSignal<TData, TContext>(TData data) where TData : struct, ISignalWithContext<TContext> 
            where TContext : SignalContext, ICancelableSignal => Instance.RaiseCancelableSignal<TData, TContext>(data);
    }
}
