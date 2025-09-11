using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        public readonly struct Handle
        {
            internal Handle(int id)
            {
                Id = id;
            }

            private int Id { get; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryCancel()
            {
                return ThreadGate.TryCancel(Id);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Cancel()
            {
                ThreadGate.Cancel(Id);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool IsValid()
            {
                return ThreadGate.IsValid(Id);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool IsDone()
            {
                return ThreadGate.IsDone(Id);
            }

            public async Task AsTask(float checkInterval = 0.1f, float timeout = 0)
            {
                var id = Id;
                var millisecondsDelay = (int)(checkInterval * 1000);
                var timeoutMilliseconds = timeout <= 0 ?  int.MaxValue : (int)(timeout * 1000);
                
                while (!ThreadGate.IsDone(id))
                {
                    if (timeoutMilliseconds <= 0) return;
                    
                    await Task.Delay(millisecondsDelay);
                    timeoutMilliseconds -= millisecondsDelay;
                }
            }
            
            public async UniTask AsUniTask(float checkInterval = 0.1f, float timeout = 0)
            {
                var id = Id;
                var millisecondsDelay = (int)(checkInterval * 1000);
                var timeoutMilliseconds = timeout <= 0 ?  int.MaxValue : (int)(timeout * 1000);
                
                while (!ThreadGate.IsDone(id))
                {
                    if (timeoutMilliseconds <= 0) return;
                    
                    await UniTask.Delay(millisecondsDelay);
                    timeoutMilliseconds -= millisecondsDelay;
                }
            }
        }
    }
}