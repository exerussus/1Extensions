
using System;
using UnityEngine;

namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public static partial class DelayedAction
    {
        /// <summary> Создает отложенное действие, которое вызовется как только все условия будут выполнены, а затем повторится после задержки. </summary>
        /// <param name="id">Уникальный номер операции, с помощью которого можно регулировать дублирование операций, а так же досрочно отменять их.</param>
        /// <param name="checkDelay">Задержка между вызовов проверок.</param>
        /// <param name="timeoutDelay">Сколько в секундах данная операция остается валидной.</param>
        /// <param name="validationFunc">Валидация операции на возможность, или актуальность выполнения. В случае непрохождения - операция удаляется.</param>
        /// <param name="conditionFunc">Условие, при котором выполняется операция.</param>
        /// <param name="action">Сама операция.</param>
        /// <param name="cycleDelay">Задержка после успешного выполнения действия.</param>
        private static void CreateOperation(
            int id,
            float checkDelay,
            float timeoutDelay,
            Func<bool> validationFunc,
            Func<bool> conditionFunc,
            Action action,
            float cycleDelay)
        {
            var operation = GetFromPool();

            operation.IsActive = true;
            operation.Id = id;
            operation.Timeout = timeoutDelay > 0 ? _time + timeoutDelay : float.MaxValue;
            operation.Delay = checkDelay;
            operation.IsCycle = cycleDelay > 0;
            operation.CycleDelay = cycleDelay;
            operation.ConditionFunc = conditionFunc ?? (() => true);
            operation.ValidationFunc = validationFunc ?? (() => true);
            operation.Action = action;
            ToCreate.Add(operation.Id, operation);
        }
        private static void UpdateRemovingOperations()
        {
            foreach (var id in ToRelease)
            {
                if (DictInWork.TryGetValue(id, out var operation)) Release(operation);
            }

            ToRelease.Clear();
        }

        private static void UpdateCreatingOperations()
        {
            foreach (var operation in ToCreate.Values)
            {
                operation.NextCheckTime = _time + operation.Delay;
                DictInWork[operation.Id] = operation;
            }
            
            ToCreate.Clear();
        }

        private static void UpdateWorkingOperations()
        {
            foreach (var operation in DictInWork.Values)
            {
                if (ToRelease.Contains(operation.Id)) continue;
                if (!operation.IsActive) continue;
                
                if (operation.NextCheckTime > _time) continue;

                if (operation.Timeout < _time)
                {
                    PrepareToRelease(operation.Id);
                    continue;
                }

                if (!operation.ValidationFunc())
                {
                    PrepareToRelease(operation.Id);
                    continue;
                }

                if (operation.ConditionFunc())
                {
                    operation.Action.Invoke();
                    
                    if (operation.IsCycle) operation.NextCheckTime = _time + operation.CycleDelay;
                    else PrepareToRelease(operation.Id);
                }
                else operation.NextCheckTime = _time + operation.Delay;
            }
        }
        
        private static Operation GetFromPool()
        {
            return Pool.TryDequeue(out var result) ? result : new Operation { IsActive = true };
        }

        private static void InvokeForceOperation(int id)
        {
            if (DictInWork.TryGetValue(id, out var delayedAction)) delayedAction.NextCheckTime = 0;
        }
        
        private static void PrepareToRelease(int id)
        {
            if (DictInWork.ContainsKey(id)) ToRelease.Add(id);
        }

        private static void StartOperation(int id)
        {
            if (DictInWork.TryGetValue(id, out var delayedAction))
            {
                if (delayedAction.IsActive) return;
                delayedAction.NextCheckTime = _time + delayedAction.Delay;
                delayedAction.IsActive = true;
            }
        }

        private static void ResetOperation(int id)
        {
            if (DictInWork.TryGetValue(id, out var delayedAction))
            {
                delayedAction.NextCheckTime = _time + delayedAction.Delay;
                delayedAction.IsActive = true;
            }
        }
        
        private static void StopOperation(int id)
        {
            if (DictInWork.TryGetValue(id, out var delayedAction)) delayedAction.IsActive = false;
        }

        private static void Release(Operation operation)
        {
            var id = operation.Id;
            DictInWork.Remove(operation.Id);
            operation.IsActive = true;
            operation.Id = 0;
            operation.NextCheckTime = 0;
            operation.Timeout = 0;
            operation.Delay = 0;
            operation.CycleDelay = 0;
            operation.IsCycle = false;
            operation.ValidationFunc = null;
            operation.ConditionFunc = null;
            operation.Action = null;
            Pool.Enqueue(operation);
        }
    }
}