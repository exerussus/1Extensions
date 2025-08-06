using System;

namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public partial class DelayedAction
    {
        public class Operation
        {
            /// <summary> True, если операция не помечена, как ожидающая старта </summary>
            public bool IsActive;
            /// <summary> Уникальный ID операции </summary>
            public int Id;
            /// <summary> Время следующей проверки </summary>
            public float NextCheckTime;
            /// <summary> Время таймаута </summary>
            public float Timeout;
            /// <summary> Время задержки перед следующей проверкой, если проверка не была пройдена </summary>
            public float Delay;
            /// <summary> Время ожидания перед следующей проверкой, в случае успешной проверки. </summary>
            public float CycleDelay;
            /// <summary> True, если после проверки происходит повторная проверка </summary>
            public bool IsCycle;
            /// <summary> Функция валидации. Если возвращает False, то операция будет уничтожена  </summary>
            public Func<bool> ValidationFunc;
            /// <summary> Функция проверки. Если возвращает True, то вызывается Action и операция удаляется\уходит на следующий цикл </summary>
            public Func<bool> ConditionFunc;
            /// <summary> Действие, которое сработает после ожидания, проверки валидации и условия. </summary>
            public Action Action;
        }
    }
}