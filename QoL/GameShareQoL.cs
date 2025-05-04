using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class GameShareQoL
    {
        public static GameShare Instance;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            Instance = new();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetSharedObject<T>()
        {
            return Instance.GetSharedObject<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T2 GetSharedObject<T1, T2>()
        {
            return Instance.GetSharedObject<T1, T2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetSharedObject<T>(string id)
        {
            return Instance.GetSharedObject<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetSharedObject<T>(ref T sharedObject)
        {
            Instance.GetSharedObject(ref sharedObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetSharedObject<T>(Type type)
        {
            return Instance.GetSharedObject<T>(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSharedObject<T>(Type type, T sharedObject)
        {
            Instance.AddSharedObject<T>(type, sharedObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSharedObject<T>(string id, T sharedObject)
        {
            Instance.AddSharedObject<T>(id, sharedObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSharedObject<T>(T sharedObject)
        {
            Instance.AddSharedObject<T>(sharedObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSharedObjects(IEnumerable<object> addingSharedObjects)
        {
            Instance.AddSharedObject(addingSharedObjects);
        }
        
    }
}