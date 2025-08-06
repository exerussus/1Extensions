
using System.Runtime.CompilerServices;

namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public partial class DelayedAction
    {
        public readonly struct Handle
        {
            public Handle(int id)
            {
                Id = id;
            }

            private int Id { get; }

            /// <summary> Принудительно выполняет задачу без ожидания, но с валидацией и проверкой. </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void InvokeForce()
            {
                DelayedAction.InvokeForceOperation(Id);
            }
            
            /// <summary> Запускает операцию, если она была остановлена. Задача запущена по-умолчанию. </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Start()
            {
                DelayedAction.StartOperation(Id);
            }
            
            /// <summary> Останавливает операцию, не отменяя её. </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Stop()
            {
                DelayedAction.StopOperation(Id);
            }
            
            /// <summary> Сбрасывает время задержки операции до стандартного ожидания, и делает операцию активной. </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                DelayedAction.ResetOperation(Id);
            }
            
            /// <returns>True, если операция всё ещё выполняется. </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool IsValid()
            {
                return Id != 0 && (DelayedAction.ToCreate.ContainsKey(Id) || DelayedAction.DictInWork.ContainsKey(Id) && !DelayedAction.ToRelease.Contains(Id));
            }
            
            /// <summary> Отменяет задачу. </summary>
            /// <returns>True, если операция была отменена. </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryCancel()
            {
                if (DelayedAction.DictInWork.ContainsKey(Id))
                {
                    DelayedAction.PrepareToRelease(Id);
                    return true;
                }

                return false;
            }
            
            /// <summary> Отменяет задачу. </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Cancel()
            {
                DelayedAction.PrepareToRelease(Id);
            }
        }
    }
}