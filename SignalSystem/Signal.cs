using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exerussus._1Extensions.Scripts.Extensions;
using Exerussus._1Extensions.SmallFeatures;
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
        private Dictionary<Type, object> _shotLstnrWithIds = new Dictionary<Type, object>();
        private Dictionary<Type, object> _cancelableListeners = new();
        private Dictionary<Type, object> _longListeners = new();
        
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
        
        /// <summary> Вызывает сигнал с фильтрацией по long id. </summary>
        public void RegistryRaiseLong<T>(long id, T data) where T : struct
        {
            var type = typeof(T);
            if (IsLogEnabled) Debug.Log($"{type}");

            if (!_listeners.TryGetValue(type, out var longDict)) return;
            
            var longs = (Dictionary<long, List<Action<T>>>)longDict;
            if (!longs.TryGetValue(id, out var actions)) return;
            
            for (var index = actions.Count - 1; index >= 0; index--)
            {
                actions[index].Invoke(data);
            }
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

            if (_shotLstnrWithIds.TryPop(type, out var shotActionIdList))
            {
                var actions = (Dictionary<string, Action<T>>)shotActionIdList;
                foreach (var action in actions.Values) action.Invoke(data);
            }
        }
        
        /// <summary> Регистрирует сигнал содержащий контекст для работы в асинхронном режиме. </summary>
        /// <param name="data"> Структура наследник IAsyncSignal содержащая SignalContext. </param>
        /// <param name="delay"> Задержка в миллисекундах. </param>
        /// <param name="timeout"> Таймаут в миллисекундах. </param>
        /// <returns> Возвращает контекст структуры при изменении в ней SignalRequestState. </returns>
        public async Task<TContext> RegistryRaiseAsync<TData, TContext>(TData data, int delay = 100, int timeout = 10000) 
            where TData : struct, ISignalWithAsyncContext<TContext>
            where TContext : AsyncSignalContext, new()
        {
            var type = typeof(TData);
            if (data.Context == null) data.Context = new TContext();
            data.Context.State = AsyncSignalState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == AsyncSignalState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = AsyncSignalState.Timeout;
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
            where TData : struct, ISignalWithAsyncContext<ResultContext>
        {
            var type = typeof(TData);
            if (data.Context == null) data.Context = new ResultContext();
            data.Context.State = AsyncSignalState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == AsyncSignalState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = AsyncSignalState.Timeout;
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
            where TData : struct, ISignalWithAsyncContext<TContext>
            where TContext : AsyncSignalContext, new()
        {
            var type = typeof(TData);
            var data = new TData();
            if (data.Context == null) data.Context = new TContext();
            data.Context.State = AsyncSignalState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == AsyncSignalState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = AsyncSignalState.Timeout;
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
        public async Task<ResultContext> RegistryRaiseAsync<TData>(int delay = 100, int timeout = 10000) where TData : struct, ISignalWithAsyncContext<ResultContext>
        {
            var type = typeof(TData);
            var data = new TData();
            if (data.Context == null) data.Context = new ResultContext();
            Tracer.Ping($"Created new context for {typeof(TData).Name} | hash : {data.Context.GetHashCode()}");
            data.Context.State = AsyncSignalState.Awaiting;
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);

            var endTime = DateTime.Now.Millisecond + timeout;
            while (data.Context.State == AsyncSignalState.Awaiting)
            {
                if (DateTime.Now.Millisecond > endTime)
                {
                    data.Context.State = AsyncSignalState.Timeout;
                    break;
                }
                
                #if UNITY_EDITOR
                
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    data.Context.State = AsyncSignalState.Timeout;
                    break;
                }
                
                #endif
                
                Tracer.Ping($"{typeof(TData).Name} not ready. State : {data.Context.State.ToString()}");
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
            if (!_listeners.TryGetValue(type, out var actionList) || actionList is not List<Action<T>> list) _listeners[type] = list = new List<Action<T>>();
            list.Add(action);
        }

        public void SubscribeFilter<T>(long id, Action<T> action) where T : struct
        {
            var type = typeof(T);
            
            if (!_longListeners.TryGetValue(type, out var raw) || raw is not Dictionary<long, List<Action<T>>> dict)
            {
                dict = new Dictionary<long, List<Action<T>>>();
                _longListeners[type] = dict;
            }

            if (!dict.TryGetValue(id, out var listeners))
            {
                listeners = new List<Action<T>>();
                dict[id] = listeners;
            }
            
            listeners.Add(action);
        }

        public void UnsubscribeFilter<T>(long id, Action<T> action) where T : struct
        {
            var type = typeof(T);
            if (!_longListeners.TryGetValue(type, out var raw)) return;
            var dict = (Dictionary<long, List<Action<T>>>)raw;
            if (dict.TryGetValue(id, out var actions)) actions.Remove(action);
        }

        public void SubscribeShot<T>(Action<T> action) where T : struct
        {
            var type = typeof(T);
            if (!_shotListeners.TryGetValue(type, out var actionList) || actionList is not List<Action<T>> list) _shotListeners[type] = list = new List<Action<T>>();
            list.Add(action);
        }

        public void SubscribeShot<T>(string id, Action<T> action) where T : struct
        {
            var type = typeof(T);
            if (!_shotLstnrWithIds.TryGetValue(type, out var actionDict) || actionDict is not Dictionary<string, Action<T>> dict) _shotLstnrWithIds[type] = dict = new Dictionary<string, Action<T>>();
            dict[id] = action;
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
        
        public void SubscribeCancelable<TData, TContext>(Action<TData> action) 
            where TData : struct, ISignalWithContext<TContext>
            where TContext : SignalContext, ICancelableSignal
        {
            var type = typeof(TData);
            if (!_cancelableListeners.TryGetValue(type, out var actionList) || actionList is not List<Action<TData>> list) _cancelableListeners[type] = list = new List<Action<TData>>();
            list.Add(action);
        }

        public void UnsubscribeCancelable<TData, TContext>(Action<TData> action) 
            where TData : struct, ISignalWithContext<TContext>
            where TContext : SignalContext, ICancelableSignal
        {
            var type = typeof(TData);
            if (_cancelableListeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<TData>>)actionList;
                actions.Remove(action);
            }
        }
        
        public void RaiseCancelableSignal<TData, TContext>(TData data) where TData : struct, ISignalWithContext<TContext> where TContext : SignalContext, ICancelableSignal
        {
            var type = typeof(TData);
            if (_cancelableListeners.ContainsKey(type) && _cancelableListeners[type] is List<Action<TData>> list)
            {
                var flag = data.Context.Flag;
                foreach (var action in list)
                {
                    action?.Invoke(data);
                    if (data.Context.Flag != flag) break;
                }
            }
        }
    }

    public enum AsyncSignalState
    {
        Awaiting,
        Success,
        Fail,
        Timeout
    }

    public abstract class AsyncSignalContext
    {
        private AsyncSignalState _state = AsyncSignalState.Awaiting;

        public AsyncSignalState State
        {
            get => _state;
            set
            {
                //Tracer.Ping($"{GetType().Name} CONTEXT STATE CHANGED : {_state} => {value}");
                _state = value;
            }
        }

        public void Done()
        {
            lock (this)
            {
                State = AsyncSignalState.Success;
                //Tracer.Ping($"{GetType().Name} CONTEXT DONE");
            }
        }
    }

    public class ResultContext : AsyncSignalContext
    {
        public Dictionary<string, object> InputParameters { get; private set; } = new();
        public Dictionary<string, object> OutputParameters { get; private set; } = new();
    }

    public class PackedObject
    {
        public PackedObject() { }

        public PackedObject(object o) { Object = o; }

        public object Object { get; set; }
    }

    public class PackedObject<T>
    {
        public PackedObject() { }

        public PackedObject(T o) { Object = o; }
        public T Object { get; set; }
    }

    public interface ISignalWithAsyncContext<T> where T : AsyncSignalContext
    {
        public T Context { get; set; }
    }

    public interface ISignalWithContext<T> where T : SignalContext
    {
        public T Context { get; set; }
    }
    
    public abstract class SignalContext
    {
        
    }

    public interface ICancelableSignal
    {
        public bool Flag { get; set; }
    }
}