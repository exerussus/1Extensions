﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exerussus._1Extensions.Scripts.Extensions;

namespace Exerussus._1Extensions.SignalSystem
{
    public class AsyncSignalBuilder<T> where T : struct, ISignalWithAsyncContext<ResultContext>
    {
        public Signal Signal;
        public T Data;
    }
    
    public static class SignalBuilderExtension
    {
        private static Dictionary<Type, List<object>> _builders = new();

        private static AsyncSignalBuilder<T> GetInstance<T>() where T : struct, ISignalWithAsyncContext<ResultContext>
        {
            var type = typeof(T);
            if (!_builders.TryGetValue(type, out var resultList))
            {
                resultList = new List<object>();
                _builders[type] = resultList;
            }

            AsyncSignalBuilder<T> result;
            
            if (resultList.Count == 0)
            {
                result = new AsyncSignalBuilder<T>();
            }
            else
            {
                result = resultList.PopLast() as AsyncSignalBuilder<T>;
            }

#if UNITY_EDITOR
            if (result == null) throw new NullReferenceException();
#endif
            result.Data.Context = new ResultContext();
            return result;
        }

        private static void Release<T>(AsyncSignalBuilder<T> instance) where T : struct, ISignalWithAsyncContext<ResultContext>
        {
            var type = typeof(T);
            if (!_builders.TryGetValue(type, out var resultList))
            {
                resultList = new List<object>();
                _builders[type] = resultList;
            }
            
            resultList.Add(instance);
        }
        
        public static AsyncSignalBuilder<T> CreateAsync<T>(this Signal signal) where T : struct, ISignalWithAsyncContext<ResultContext>
        {
            var instance = GetInstance<T>();
            instance.Signal = signal;
            return instance;
        }

        public static AsyncSignalBuilder<T> AddInputParam<T>(this AsyncSignalBuilder<T> instance, string paramKey, object value) where T : struct, ISignalWithAsyncContext<ResultContext>
        {
            instance.Data.Context.InputParameters[paramKey] = value;
            return instance;
        }

        public static AsyncSignalBuilder<T> AddOutputParam<T>(this AsyncSignalBuilder<T> instance, string paramKey, object value) where T : struct, ISignalWithAsyncContext<ResultContext>
        {
            instance.Data.Context.OutputParameters[paramKey] = value;
            return instance;
        }

        public static async Task<ResultContext> Invoke<T>(this AsyncSignalBuilder<T> instance) where T : struct, ISignalWithAsyncContext<ResultContext>
        {
            var result = await instance.Signal.RegistryRaiseAsync(instance.Data);

            Release(instance);
            
            return result;
        }

        public static async Task<ResultContext> Invoke<T>(this AsyncSignalBuilder<T> instance, int delay, int timeout) where T : struct, ISignalWithAsyncContext<ResultContext>
        {
            var result = await instance.Signal.RegistryRaiseAsync(instance.Data, delay, timeout);

            Release(instance);
            
            return result;
        }
    }
}