﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        /// <summary>
        /// Вызывает сигнал 
        /// </summary>
        public void RegistryRaise<T>(T data) where T : struct
        {
            var type = typeof(T);
            if (IsLogEnabled) Debug.Log($"{type}");

            if (_listeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<T>>)actionList;
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

            if (_listeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<TData>>)actionList;
                for (var index = actions.Count - 1; index >= 0; index--)
                {
                    actions[index].Invoke(data);
                }
            }

            var startTime = DateTime.Now;
            while (data.Context.State == SignalRequestState.Awaiting)
            {
                if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
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

            if (_listeners.TryGetValue(type, out var actionList))
            {
                var actions = (List<Action<T>>)actionList;
                for (var index = actions.Count - 1; index >= 0; index--)
                {
                    actions[index].Invoke(data);
                }
            }
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

    public interface IAsyncSignal<T> where T : SignalContext
    {
        public T Context { get; set; }
    }
}