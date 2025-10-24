using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Exerussus._1Extensions.Scripts.Extensions;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        public static partial class FuncBuilding<T>
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
                    return ThreadGate.FuncBuilding<T>.TryCancel(Id);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public void Cancel()
                {
                    ThreadGate.FuncBuilding<T>.Cancel(Id);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool IsValid()
                {
                    return ThreadGate.FuncBuilding<T>.IsValid(Id);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool IsDone()
                {
                    return ThreadGate.FuncBuilding<T>.IsDone(Id);
                }

                public async Task<T> AsTask(float checkInterval = 0.1f, float timeout = 0)
                {
                    var id = Id;
                    var millisecondsDelay = (int)(checkInterval * 1000);
                    var timeoutMilliseconds = timeout <= 0 ? int.MaxValue : (int)(timeout * 1000);

                    while (!ThreadGate.FuncBuilding<T>.IsDone(id))
                    {
                        if (_cts.Token.IsCancellationRequested) return default;
                        if (timeoutMilliseconds <= 0) return default;

                        await Task.Delay(millisecondsDelay, cancellationToken: _cts.Token);
                        timeoutMilliseconds -= millisecondsDelay;
                    }

                    return ThreadGate.FuncBuilding<T>.Results.Pop(id);
                }

                public async UniTask<T> AsUniTask(float checkInterval = 0.1f, float timeout = 0)
                {
                    var id = Id;
                    var millisecondsDelay = (int)(checkInterval * 1000);
                    var timeoutMilliseconds = timeout <= 0 ? int.MaxValue : (int)(timeout * 1000);
                    SetReturnable(id);

                    while (!ThreadGate.FuncBuilding<T>.IsDone(id))
                    {
                        if (_cts.Token.IsCancellationRequested) return default;
                        if (timeoutMilliseconds <= 0) return default;

                        await UniTask.Delay(millisecondsDelay, cancellationToken: _cts.Token);
                        timeoutMilliseconds -= millisecondsDelay;
                    }
                    
                    return ThreadGate.FuncBuilding<T>.Results.Pop(id);
                }
            }
        }
    }
}