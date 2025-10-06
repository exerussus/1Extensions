using System;
using System.Runtime.CompilerServices;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        public static partial class FuncBuilding<T>
        {
            private static int _freeJobIndex = 1;
            private static int _freeBuilderIndex = 1;
            private static readonly object JobIndexLock = new();
            private static readonly object BuilderIndexLock = new();
            private static bool _isInit = false;
            
            public readonly struct Builder
            {
                private Builder(int id)
                {
                    _id = id;
                }

                public static Builder Create(Func<T> action)
                {
                    int id = 0;

                    lock (BuilderIndexLock)
                    {
                        id = _freeBuilderIndex++;
                    }

                    Buffer.Create(id, action);

                    lock (CreateLock)
                    {
                        if (!_isInit)
                        {
                            _isInit = true;
                            #if UNITY_EDITOR
                            EditorDispose += () =>
                            {
                                _isInit = false;
                                _isUpdating = false;
                            };
                            #endif
                        }
                        
                        if (!_isUpdating)
                        {
                            _isUpdating = true;
                            ThreadGate._funcBuildingUpdate += UpdateFuncBuilding;
                        }
                    }
                    
                    return new Builder(id);
                }

                private readonly int _id;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder WithDelay(float delay)
                {
                    ThreadGate.FuncBuilding<T>.SetDelay(_id, delay);
                    return this;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder WithProtection()
                {
                    ThreadGate.FuncBuilding<T>.SetProtection(_id);
                    return this;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder Preserve()
                {
                    ThreadGate.FuncBuilding<T>.SetPreserve(_id);
                    return this;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Builder Break()
                {
                    ThreadGate.FuncBuilding<T>.Break(_id);
                    return this;
                }

                public Handle Run()
                {
                    if (!ThreadGate.FuncBuilding<T>.GetIsValid(_id)) return default;

                    int jobId;
                    lock (JobIndexLock)
                    {
                        jobId = _freeJobIndex++;
                    }

                    var handle = new Handle(jobId);

                    ThreadGate.FuncBuilding<T>.BakeJob(_id, jobId);

                    return handle;
                }
            }
        }
    }
}