using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exerussus._1Extensions.Async
{
    public static class TaskUtils
    {
        private const int DefaultCheckInterval = 100;
        
        public static async Task WaitUntilAsync(Func<bool> condition)
        {
            while (!condition())
            {
                await Task.Delay(DefaultCheckInterval);
            }
        }
        
        public static async Task WaitUntilAsync(Func<bool> condition, int checkInterval)
        {
            while (!condition())
            {
                await Task.Delay(checkInterval);
            }
        }
        
        public static async Task WaitUntilAsync(Func<bool> condition, int checkInterval, int timeout)
        {
            while (!condition())
            {
                if (timeout <= 0) return;
                await Task.Delay(checkInterval);
                timeout -= checkInterval;
            }
        }
        
        public static async Task WaitUntilAsync(Func<bool>[] conditions)
        {
            while (!conditions.All(c => c()))
            {
                await Task.Delay(DefaultCheckInterval);
            }
        }
        
        public static async Task WaitUntilAsync(List<Func<bool>> conditions)
        {
            while (!conditions.All(c => c()))
            {
                await Task.Delay(DefaultCheckInterval);
            }
        }
    }
}