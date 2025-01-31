using System;

namespace Exerussus.PriorityMachineFeature
{
    public class PriorityState<TEntity, TSharedData>
    {
        public PriorityState(string id, Func<TEntity, TSharedData, int, int> calculatePriority, Action<TEntity, TSharedData> enter, Action<TEntity, TSharedData> update, Action<TEntity, TSharedData> exit)
        {
            Id = id;
            CalculatePriority = calculatePriority;
            OnEnter = enter;
            OnUpdate = update;
            OnExit = exit;
        }

        public readonly string Id;
        /// <summary> Func (entity, sharedData, current priority, return new priority) </summary>
        public readonly Func<TEntity, TSharedData, int, int> CalculatePriority;
        public readonly Action<TEntity, TSharedData> OnEnter;
        public readonly Action<TEntity, TSharedData> OnUpdate;
        public readonly Action<TEntity, TSharedData> OnExit;
    }
}