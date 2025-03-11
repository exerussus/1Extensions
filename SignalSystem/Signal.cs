using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exerussus._1Extensions.Scripts.Extensions;
using UnityEngine;

namespace Exerussus._1Extensions.SignalSystem
{
    public class Signal
    {
        public Signal(bool isLogsEnable = false)
        {
            IsLogEnabled = isLogsEnable;
        }
        
        private bool IsLogEnabled { get; set; }

        private Dictionary<Type, object> _listeners = new Dictionary<Type, object>();
        private Dictionary<Type, object> _shotListeners = new Dictionary<Type, object>();
        
        /// <summary> Вызывает сигнал. </summary>
        public void RegistryRaise<T>(T data) where T : struct
        {
            var type = typeof(T);
            if (IsLogEnabled) Debug.Log($"{type}");

            FindAndInvokeAction(ref data, type);
        }

        /// <summary> Вызывает сигнал без аргументов. </summary>
        public void RegistryRaise<T>() where T : struct
        {
            var type = typeof(T);
            var data = new T();
            if (IsLogEnabled) Debug.Log($"{type}");

            FindAndInvokeAction(ref data, type);
        }

        private void FindAndInvokeAction<T>(ref T data, Type type) where T : struct
        {
            if (_listeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<T>>)actionList;
                for (var index = actions.Count - 1; index >= 0; index--)
                {
                    actions[index].Invoke(data);
                }
            }

            if (_shotListeners.TryPop(type, out var shotActionList))
            {
                var actions = (List<Action<T>>)shotActionList;
                for (var index = actions.Count - 1; index >= 0; index--)
                {
                    actions[index].Invoke(data);
                }
            }
        }
        
        /// <summary> Регистрирует сигнал содержащий контекст для работы в асинхронном режиме. </summary>
        /// <param name="data"> Структура наследник IAsyncSignal содержащая SignalContext. </param>
        /// <param name="delay"> Задержка в миллисекундах. </param>
        /// <param name="timeout"> Таймаут в миллисекундах. </param>
        /// <returns> Возвращает контекст структуры при изменении в ней SignalRequestState. </returns>
        public async Task<TContext> RegistryRaiseAsync<TData, TContext>(TData data, int delay = 100, int timeout = 10000) 
            where TData : struct, IAsyncSignal<TContext>
            where TContext : SignalContext, new()
        {
            var type = typeof(TData);
            if (data.Context == null) data.Context = new TContext();
            data.Context.State = SignalRequestState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == SignalRequestState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = SignalRequestState.Timeout;
                    break;
                }
                await Task.Delay(delay);
            }
    
            return data.Context;
        }
        
        /// <summary> Регистрирует сигнал содержащий контекст для работы в асинхронном режиме. </summary>
        /// <param name="data"> Структура наследник IAsyncSignal содержащая SignalContext. </param>
        /// <param name="delay"> Задержка в миллисекундах. </param>
        /// <param name="timeout"> Таймаут в миллисекундах. </param>
        /// <returns> Возвращает контекст структуры при изменении в ней SignalRequestState. </returns>
        public async Task<ResultContext> RegistryRaiseAsync<TData>(TData data, int delay = 100, int timeout = 10000) 
            where TData : struct, IAsyncSignal<ResultContext>
        {
            var type = typeof(TData);
            if (data.Context == null) data.Context = ResultContext.GetInstance();
            data.Context.State = SignalRequestState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == SignalRequestState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = SignalRequestState.Timeout;
                    break;
                }
                await Task.Delay(delay);
            }
    
            return data.Context;
        }
        
        /// <summary> Регистрирует сигнал содержащий контекст для работы в асинхронном режиме. </summary>
        /// <param name="data"> Структура наследник IAsyncSignal содержащая SignalContext. </param>
        /// <param name="delay"> Задержка в миллисекундах. </param>
        /// <param name="timeout"> Таймаут в миллисекундах. </param>
        /// <returns> Возвращает контекст структуры при изменении в ней SignalRequestState. </returns>
        public async Task<TContext> RegistryRaiseAsync<TData, TContext>(int delay = 100, int timeout = 10000) 
            where TData : struct, IAsyncSignal<TContext>
            where TContext : SignalContext, new()
        {
            var type = typeof(TData);
            var data = new TData();
            if (data.Context == null) data.Context = new TContext();
            data.Context.State = SignalRequestState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == SignalRequestState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = SignalRequestState.Timeout;
                    break;
                }
                await Task.Delay(delay);
            }
    
            return data.Context;
        }
        
        /// <summary> Регистрирует сигнал содержащий контекст для работы в асинхронном режиме. </summary>
        /// <param name="data"> Структура наследник IAsyncSignal содержащая SignalContext. </param>
        /// <param name="delay"> Задержка в миллисекундах. </param>
        /// <param name="timeout"> Таймаут в миллисекундах. </param>
        /// <returns> Возвращает контекст структуры при изменении в ней SignalRequestState. </returns>
        public async Task<ResultContext> RegistryRaiseAsync<TData>(int delay = 100, int timeout = 10000) 
            where TData : struct, IAsyncSignal<ResultContext>
        {
            var type = typeof(TData);
            var data = new TData();
            if (data.Context == null) data.Context = ResultContext.GetInstance();
            data.Context.State = SignalRequestState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == SignalRequestState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = SignalRequestState.Timeout;
                    break;
                }
                await Task.Delay(delay);
            }
    
            return data.Context;
        }
        
        /// <summary>
        /// Вызывает сигнал без создания копии на входе
        /// </summary>
        public void RegistryRaise<T>(ref T data) where T : struct
        {
            var type = typeof(T);
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);
        }

        public void Subscribe<T>(Action<T> action) where T : struct
        {
            var type = typeof(T);
            if (!_listeners.TryGetValue(type, out var actionList))
            {
                actionList = new List<Action<T>>();
                _listeners[type] = actionList;
            }
            ((List<Action<T>>)actionList).Add(action);
        }

        public void SubscribeShot<T>(Action<T> action) where T : struct
        {
            var type = typeof(T);
            if (!_shotListeners.TryGetValue(type, out var actionList))
            {
                actionList = new List<Action<T>>();
                _shotListeners[type] = actionList;
            }
            ((List<Action<T>>)actionList).Add(action);
        }

        public void Unsubscribe<T>(Action<T> action) where T : struct
        {
            var type = typeof(T);
            if (_listeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<T>>)actionList;
                actions.Remove(action);
            }
        }
    }

    public enum SignalRequestState
    {
        Awaiting,
        Success,
        Fail,
        Timeout
    }

    public abstract class SignalContext
    {
        public SignalRequestState State { get; set; }
    }

    public class ResultContext : SignalContext, IDisposable
    {
        public Dictionary<string, object> InputParameters { get; private set; } = new();
        public Dictionary<string, object> OutputParameters { get; private set; } = new();
        
        private static Queue<ResultContext> _pool = new();

        public void Release()
        {
            ReleaseInstance(this);
        }
        
        public static ResultContext GetInstance()
        {
            if (!_pool.TryDequeue(out var instance))
            {
                instance = new ResultContext();
            }
            
            return instance;
        }
        
        public static void ReleaseInstance(ResultContext instance)
        {
            instance.InputParameters.Clear();
            instance.OutputParameters.Clear();
            instance.State = SignalRequestState.Awaiting;
            _pool.Enqueue(instance);
        }

        public void Dispose()
        {
            ReleaseInstance(this);
        }
    }

    public interface IAsyncSignal<T> where T : SignalContext
    {
        public T Context { get; set; }
    }
}