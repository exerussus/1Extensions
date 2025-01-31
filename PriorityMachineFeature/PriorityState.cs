using System;

namespace Exerussus.PriorityMachineFeature
{
    public class PriorityState<TEntity, TSharedData>
    {
        public PriorityState(
            string id, 
            Func<TEntity, TSharedData, int, int> calculatePriority, 
            Action<TEntity, TSharedData> onCreate, 
            Action<TEntity, TSharedData> enter, 
            Action<TEntity, TSharedData> update, 
            Action<TEntity, TSharedData> exit,
            Action<TEntity, TSharedData> onRemove)
        {
            Id = id;
            CalculatePriority = calculatePriority;
            OnCreate = onCreate;
            OnEnter = enter;
            OnUpdate = update;
            OnExit = exit;
            OnRemove = onRemove;
        }

        public readonly string Id;
        /// <summary> Func (entity, sharedData, current priority, return new priority) </summary>
        public readonly Func<TEntity, TSharedData, int, int> CalculatePriority;
        public readonly Action<TEntity, TSharedData> OnCreate;
        public readonly Action<TEntity, TSharedData> OnEnter;
        public readonly Action<TEntity, TSharedData> OnUpdate;
        public readonly Action<TEntity, TSharedData> OnExit;
        public readonly Action<TEntity, TSharedData> OnRemove;
    }
}