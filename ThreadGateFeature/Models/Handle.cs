using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Exerussus._1Extensions.Async;

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

            public async Task Wait(float checkInterval = 0.1f, float timeout = 0)
            {
                var id = Id;
                if (timeout > 0) await TaskUtils.WaitUntilAsync(() => ThreadGate.IsDone(id), (int)(checkInterval * 1000), (int)(timeout * 1000));
                else await TaskUtils.WaitUntilAsync(() => ThreadGate.IsDone(id), (int)(checkInterval * 1000));
            }
        }
    }
}