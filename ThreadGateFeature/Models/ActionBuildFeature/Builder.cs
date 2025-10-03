using System;
using System.Runtime.CompilerServices;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        public static partial class ActionBuilding
        {
            private static int _freeJobIndex = 1;
            private static int _freeBuilderIndex = 1;
            private static readonly object JobIndexLock = new();
            private static readonly object BuilderIndexLock = new();

            public readonly struct Builder
            {
                private Builder(int id)
                {
                    _id = id;
                }

                public static Builder Create(Action action)
                {
                    int id = 0;

                    lock (BuilderIndexLock)
                    {
                        id = _freeBuilderIndex++;
                    }

                    Buffer.Create(id, action);
                    return new Builder(id);
                }

                private readonly int _id;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder WithDelay(float delay)
                {
                    ThreadGate.ActionBuilding.SetDelay(_id, delay);
                    return this;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder WithProtection()
                {
                    ThreadGate.ActionBuilding.SetProtection(_id);
                    return this;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder Preserve()
                {
                    ThreadGate.ActionBuilding.SetPreserve(_id);
                    return this;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder Break()
                {
                    ThreadGate.ActionBuilding.Break(_id);
                    return this;
                }

                public Handle Run()
                {
                    if (!ThreadGate.ActionBuilding.GetIsValid(_id)) return default;

                    int jobId;
                    lock (JobIndexLock)
                    {
                        jobId = _freeJobIndex++;
                    }

                    var handle = new Handle(jobId);

                    ThreadGate.ActionBuilding.BakeJob(_id, jobId);

                    return handle;
                }
            }
        }
    }
}