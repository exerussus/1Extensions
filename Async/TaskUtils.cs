using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Exerussus._1Extensions.Async
{
    public static class TaskUtils
    {
        private const int DefaultCheckInterval = 100;
        
        /// <summary> Ждёт, пока условие станет true. </summary>
        public static async UniTask WaitUntilCondition(this Func<bool> condition)
        {
            while (!condition())
            {
                await UniTask.Delay(DefaultCheckInterval);
            }
        }

        /// <summary> Ждёт, пока условие станет true, с заданным интервалом проверки. </summary>
        public static async UniTask WaitUntilCondition(this Func<bool> condition, int checkInterval)
        {
            while (!condition())
            {
                await UniTask.Delay(checkInterval);
            }
        }

        /// <summary> Ждёт, пока условие станет true, с заданным интервалом и таймаутом. </summary>
        public static async UniTask WaitUntilCondition(this Func<bool> condition, int checkInterval, int timeout)
        {
            while (!condition())
            {
                if (timeout <= 0) return;
                await UniTask.Delay(checkInterval);
                timeout -= checkInterval;
            }
        }

        /// <summary> Ждёт, пока все условия в массиве станут true. </summary>
        public static async UniTask WaitUntilCondition(this Func<bool>[] conditions)
        {
            if (conditions == null || conditions.Length == 0) return;

            while (true)
            {
                var all = true;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (!conditions[i]())
                    {
                        all = false;
                        break;
                    }
                }

                if (all) return;
                await UniTask.Delay(DefaultCheckInterval);
            }
        }

        /// <summary> Ждёт, пока все условия в списке станут true. </summary>
        public static async UniTask WaitUntilCondition(this List<Func<bool>> conditions)
        {
            if (conditions == null || conditions.Count == 0) return;

            while (true)
            {
                var all = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i]())
                    {
                        all = false;
                        break;
                    }
                }

                if (all) return;
                await UniTask.Delay(DefaultCheckInterval);
            }
        }
    }
}