using System;
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
    }
}