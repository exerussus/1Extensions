using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public class DelayedAction
    {
        #region INSTANCE

        private DelayedAction() {}
        private float _checkTime;
        private float _timeout;
        private float _delay;
        private string _id;
        private Func<bool> _validationFunc;
        private Func<bool> _conditionFunc;
        private Action _action;
        
        #endregion
        
        #region STATIC

        private static readonly Queue<DelayedAction> Pool = new();
        private static readonly Dictionary<string, DelayedAction> DictInWork = new();
        private static readonly List<string> KeysInWork = new();

        #region PUBLIC METHODS

        public static void Create(string id, float checkDelay, float timeoutDelay, Func<bool> validationFunc, Func<bool> conditionFunc, Action action)
        {
            if (DictInWork.ContainsKey(id)) Release(id);
            
            if (!validationFunc()) return;
            
            var instance = GetFromPool();
            
            instance._id = id;
            instance._timeout = Time.time + timeoutDelay;
            instance._delay = checkDelay;
            instance._checkTime = Time.time + checkDelay;
            instance._conditionFunc = conditionFunc;
            instance._validationFunc = validationFunc;
            instance._action = action;
            
            DictInWork[id] = instance;
            KeysInWork.Add(id);
        }
        
        public static void Cancel(string id)
        {
            Release(id);
        }

        public static void Update()
        {
            if (DictInWork.Count == 0) return;

            for (var index = KeysInWork.Count - 1; index >= 0; index--)
            {
                var id = KeysInWork[index];
                var delayedAction = DictInWork[id];

                if (delayedAction._checkTime > Time.time) continue;

                if (delayedAction._timeout < Time.time)
                {
                    Release(delayedAction);
                    continue;
                }

                if (!delayedAction._validationFunc())
                {
                    Release(delayedAction);
                    continue;
                }

                if (delayedAction._conditionFunc()) delayedAction._action?.Invoke();
                else delayedAction._checkTime = Time.time + delayedAction._delay;
            }
        }

        #endregion

        #region PRIVATE METHODS

        private static DelayedAction GetFromPool()
        {
            return Pool.TryDequeue(out var result) ? result : new DelayedAction();
        }

        private static void Release(string id)
        {
            if (DictInWork.TryGetValue(id, out var delayedAction)) Release(delayedAction);
        }

        private static void Release(DelayedAction delayedAction)
        {
            DictInWork.Remove(delayedAction._id);
            KeysInWork.Remove(delayedAction._id);
            delayedAction._id = null;
            delayedAction._action = null;
            delayedAction._validationFunc = null;
            delayedAction._conditionFunc = null;
            Pool.Enqueue(delayedAction);
        }

        #endregion

        #endregion
    }
}