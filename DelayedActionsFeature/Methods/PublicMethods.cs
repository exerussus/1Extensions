using System;
using UnityEngine;

namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public static partial class DelayedAction
    {
        /// <summary> Создает билдер операции, которую необходимо запустить методом Run после настройки. </summary>
        public static Builder Create(float checkDelay, Action action)
        {
            return Builder.CreateBuilder(checkDelay, action);
        }

        /// <summary> Обновление отложенных действий. Необходимо подключать в любом MonoBehavior. Задержка между вызовами не важна. </summary>
        public static void Update()
        {
            _time = Time.time;

            UpdateWorkingOperations();
            UpdateRemovingOperations();
            UpdateCreatingOperations();
        }
    }
}