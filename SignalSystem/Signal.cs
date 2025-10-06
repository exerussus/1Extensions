using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Exerussus._1Extensions.Scripts.Extensions;
using Exerussus._1Extensions.SmallFeatures;
using Exerussus._1Extensions.ThreadGateFeature;
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

        private readonly Dictionary<Type, object> _listeners = new();
        private readonly Dictionary<Type, object> _listenersAsync = new();
        private readonly Dictionary<Type, object> _affectors = new();
        private readonly Dictionary<Type, object> _shotListeners = new();
        private readonly Dictionary<Type, object> _shotListenersWithIds = new();
        private readonly Dictionary<Type, object> _cancelableListeners = new();
        private readonly Dictionary<Type, object> _longListeners = new();
#if UNITY_EDITOR
        internal readonly long UniqId = Guid.NewGuid().ToString().GetStableLongId();
#endif
        
        /// <summary> Вызывает сигнал. </summary>
        public void RegistryRaise<T>(T data = default)
        {
#if UNITY_EDITOR
            Editor.SignalManager.RegisterSignal<T>(this);
#endif
            var type = typeof(T);
            if (IsLogEnabled) Debug.Log($"{type}");

            FindAndInvokeAction(ref data, type);
        }

        #region AFFECTOR
        
        /// <summary> Вызывает сигнал без аргументов. </summary>
        public (bool isFound, TResult result) RegistryAffect<TAffector, TResult>(TAffector data = default) 
            where TAffector : IAffector<TResult>
        {
            var type = typeof(TAffector);
            if (IsLogEnabled) Debug.Log($"{type}");
            
            if (_affectors.TryGetValue(type, out var actionList))
            {
                var actions = (List<Func<TAffector, TResult>>)actionList;

                foreach (var t in actions)
                {
                    var result = t.Invoke(data);
                    if (result != null) return (true, result);
                }
            }
            
            return (false, default);
        }
        
        /// <summary> Вызывает сигнал без аргументов. </summary>
        public (bool isFound, List<TResult> result) RegistryAffectMany<TAffector, TResult>(TAffector data = default) 
            where TAffector : IAffector<TResult>
        {
            var type = typeof(TAffector);
            if (IsLogEnabled) Debug.Log($"{type}");
            
            if (_affectors.TryGetValue(type, out var actionList))
            {
                var result = new List<TResult>();
                var actions = (List<Func<TAffector, TResult>>)actionList;
                foreach (var t in actions)
                {
                    var resultItem = t.Invoke(data);
                    if (resultItem != null) result.Add(resultItem);
                }
                return (result.Count > 0, result);
            }
            
            return (false, null);
        }
        
        /// <summary> Вызывает сигнал без аргументов. </summary>
        public bool RegistryAffectMany<TAffector, TResult>(TAffector data, out List<TResult> result) 
            where TAffector : IAffector<TResult>
        {
            var type = typeof(TAffector);
            if (IsLogEnabled) Debug.Log($"{type}");
            
            if (_affectors.TryGetValue(type, out var actionList))
            {
                var actions = (List<Func<TAffector, TResult>>)actionList;
                if (actions.Count > 0) result = new List<TResult>();
                else
                {
                    result = null;
                    return false;
                }
                
                foreach (var t in actions)
                {
                    var resultItem = t.Invoke(data);
                    if (resultItem != null) result.Add(resultItem);
                }
                return result.Count > 0;
            }
            
            result = null;
            return false;
        }
        
        /// <summary> Вызывает сигнал без аргументов. </summary>
        public bool RegistryAffect<TAffector, TResult>(TAffector data, out TResult result) 
            where TAffector : IAffector<TResult>
        {
            var type = typeof(TAffector);
            if (IsLogEnabled) Debug.Log($"{type}");
            
            if (_affectors.TryGetValue(type, out var actionList))
            {
                var actions = (List<Func<TAffector, TResult>>)actionList;
                foreach (var f in actions)
                {
                    result = f.Invoke(data);
                    if (result != null) return true;
                }
            }
            result = default;
            return false;
        }
        
        /// <summary> Вызывает сигнал без аргументов. </summary>
        public bool RegistryAffect<TAffector, TResult>(out TResult result) 
            where TAffector : IAffector<TResult>
        {
            var type = typeof(TAffector);
            if (IsLogEnabled) Debug.Log($"{type}");
            
            if (_affectors.TryGetValue(type, out var actionList))
            {
                var actions = (List<Func<TAffector, TResult>>)actionList;
                foreach (var f in actions)
                {
                    result = f.Invoke(default);
                    if (result != null) return true;
                }
            }
            result = default;
            return false;
        }

        public void Subscribe<TAffector, TResult>(Func<TAffector, TResult> action) where TAffector : IAffector<TResult>
        {
            var type = typeof(TAffector);
            if (!_affectors.TryGetValue(type, out var actionList) || actionList is not List<Func<TAffector, TResult>> list) _affectors[type] = list = new List<Func<TAffector, TResult>>();
            list.Add(action);
        }
        
        public void Unsubscribe<TAffector, TResult>(Func<TAffector, TResult> action) where TAffector : IAffector<TResult>
        {
            var type = typeof(TAffector);
            if (_affectors.TryGetValue(type, out var actionList) && actionList is List<Func<TAffector, TResult>> list)
            {
                list.Remove(action);
            }
        }

        #endregion

        #region ASYNC

        
        public T CreateAsync<T>(T data) where T : AsyncSignal
        {
#if UNITY_EDITOR
            Editor.SignalManager.RegisterSignal<T>(this);
#endif
            var type = typeof(T);
            if (IsLogEnabled) Debug.Log($"{type}");

            ThreadGate.CreateJob(() => RunAsync(data, type)).Run();
            
            return data;
        }

        private async UniTask RunAsync<T>(T data, Type type) where T : AsyncSignal
        {
            if (_listenersAsync.TryGetValue(type, out var actionList))
            {
                var actions = (List<Func<T, UniTask>>)actionList;

                if (actions.Count == 0)
                {
                    data.Done(false);
                    return;
                }
                
                var tasks = new UniTask[actions.Count];
                for (var index = actions.Count - 1; index >= 0; index--) tasks[index] = actions[index].Invoke(data);
                await UniTask.WhenAll(tasks);
                data.Done(true);
            }
            else
            {
                data.Done(false);
            }
        }
        
        public void SubscribeAsync<T>(Func<T, UniTask> action) where T : AsyncSignal
        {
            var type = typeof(T);
            if (!_listenersAsync.TryGetValue(type, out var actionList) || actionList is not List<Func<T, UniTask>> list) _listenersAsync[type] = list = new List<Func<T, UniTask>>();
            list.Add(action);
        }
        
        public void UnsubscribeAsync<T>(Func<T, UniTask> action) where T : AsyncSignal
        {
            var type = typeof(T);
            if (_listenersAsync.TryGetValue(type, out var actionList))
            {
                var actions = (List<Func<T, UniTask>>)actionList;
                actions.Remove(action);
            }
        }

        #endregion
        
        #region Signal
        
        /// <summary> Вызывает сигнал с фильтрацией по long id. </summary>
        public void RegistryRaiseFilter<T>(long id, T data)
        {
#if UNITY_EDITOR
            Editor.SignalManager.RegisterSignal<T>(this);
#endif
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
        
        private void FindAndInvokeAction<T>(ref T data, Type type)
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

            if (_shotListenersWithIds.TryPop(type, out var shotActionIdList))
            {
                var actions = (Dictionary<string, Action<T>>)shotActionIdList;
                foreach (var action in actions.Values) action.Invoke(data);
            }
        }
        
        /// <summary>
        /// Вызывает сигнал без создания копии на входе
        /// </summary>
        public void RegistryRaise<T>(ref T data)
        {
#if UNITY_EDITOR
            Editor.SignalManager.RegisterSignal<T>(this);
#endif
            var type = typeof(T);
            if (IsLogEnabled) Debug.Log($"{type}");
            
            FindAndInvokeAction(ref data, type);
        }

        public void Subscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (!_listeners.TryGetValue(type, out var actionList) || actionList is not List<Action<T>> list) _listeners[type] = list = new List<Action<T>>();
            list.Add(action);
        }

        public void SubscribeFilter<T>(long id, Action<T> action)
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

        public void UnsubscribeFilter<T>(long id, Action<T> action)
        {
            var type = typeof(T);
            if (!_longListeners.TryGetValue(type, out var raw)) return;
            var dict = (Dictionary<long, List<Action<T>>>)raw;
            if (dict.TryGetValue(id, out var actions)) actions.Remove(action);
        }

        public void SubscribeShot<T>(Action<T> action)
        {
            var type = typeof(T);
            if (!_shotListeners.TryGetValue(type, out var actionList) || actionList is not List<Action<T>> list) _shotListeners[type] = list = new List<Action<T>>();
            list.Add(action);
        }

        public void SubscribeShot<T>(string id, Action<T> action)
        {
            var type = typeof(T);
            if (!_shotListenersWithIds.TryGetValue(type, out var actionDict) || actionDict is not Dictionary<string, Action<T>> dict) _shotListenersWithIds[type] = dict = new Dictionary<string, Action<T>>();
            dict[id] = action;
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_listeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<T>>)actionList;
                actions.Remove(action);
            }
        }
        
        public void SubscribeCancelable<TData, TContext>(Action<TData> action) 
            where TData : ISignalWithContext<TContext>
            where TContext : SignalContext, ICancelableSignal
        {
            var type = typeof(TData);
            if (!_cancelableListeners.TryGetValue(type, out var actionList) || actionList is not List<Action<TData>> list) _cancelableListeners[type] = list = new List<Action<TData>>();
            list.Add(action);
        }

        public void UnsubscribeCancelable<TData, TContext>(Action<TData> action) 
            where TData : ISignalWithContext<TContext>
            where TContext : SignalContext, ICancelableSignal
        {
            var type = typeof(TData);
            if (_cancelableListeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<TData>>)actionList;
                actions.Remove(action);
            }
        }
        
        public void RaiseCancelableSignal<TData, TContext>(TData data) where TData : ISignalWithContext<TContext> where TContext : SignalContext, ICancelableSignal
        {
#if UNITY_EDITOR
            Editor.SignalManager.RegisterSignal<TData>(this);
#endif
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

        #endregion
    }

    public interface ISignalWithContext<T> where T : SignalContext
    {
        public T Context { get; set; }
    }
    
    public abstract class SignalContext
    {
        
    }

    public interface IMatcher<TRequest, TResponse>
    {
            
    }

    public abstract class AsyncSignal
    {
        public bool IsDone { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsCanceled { get; private set; }
        private int _timeout;
        private int _timer;
        private const int Delay = 50;
        
        internal void Done(bool isSuccess)
        {
            IsDone = true;
            IsSuccess = isSuccess;
        }
        
        public void Cancel()
        {
            IsDone = true;
            IsCanceled = true;
        }
        
        public async UniTask<bool> Await(int timeout = -1)
        {
            _timeout = timeout;
            
            while (!IsDone && !IsCanceled)
            {
                if (_timeout > 0)
                {
                    _timer += Delay;
                    if (_timer >= _timeout)
                    {
                        Cancel();
                        return false;
                    }
                }
                await UniTask.Delay(Delay);
            }
            return IsSuccess;
        }
    }
        
    public interface IAffector<TResponse>
    {
            
    }

    public interface ICancelableSignal
    {
        public bool Flag { get; set; }
    }
}