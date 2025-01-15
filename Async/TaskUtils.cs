using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exerussus._1Extensions.Async
{
    public static class TaskUtils
    {
        public static async Task WaitUntilAsync(Func<bool> condition, int checkInterval = 100)
        {
            while (!condition())
            {
                await Task.Delay(checkInterval);
            }
        }
        
        public static async Task WaitUntilAsync(Func<bool>[] conditions, int checkInterval = 100)
        {
            while (!conditions.All(c => c()))
            {
                await Task.Delay(checkInterval);
            }
        }
        
        public static async Task WaitUntilAsync(List<Func<bool>> conditions, int checkInterval = 100)
        {
            while (!conditions.All(c => c()))
            {
                await Task.Delay(checkInterval);
            }
        }
    }
}