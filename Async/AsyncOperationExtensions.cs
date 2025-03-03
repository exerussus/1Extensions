using System.Threading.Tasks;
using UnityEngine;

namespace Exerussus._1Extensions.Async
{
    public static class AsyncOperationExtensions
    {
        public static async Task Await(this AsyncOperation operation)
        {
            if (operation == null)
            {
                Debug.LogError("Can't complete null operation.");
                return;
            }

            if (operation.isDone) return;

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Complete(AsyncOperation op)
            {
                op.completed -= Complete;
                tcs.TrySetResult(true);
            }

            operation.completed += Complete;

            await tcs.Task;
        }
    }
}