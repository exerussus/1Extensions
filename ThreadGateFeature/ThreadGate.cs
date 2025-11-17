
using System;
using System.Threading;
using Exerussus._1Extensions.LoopFeature;
using UnityEngine;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        internal static float Time = 0;
        private static Action _funcBuildingUpdate;
        private static CancellationTokenSource _cts = new();

#if UNITY_EDITOR
        private static Action EditorDispose;        
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            ExerussusLoopHelper.OnUpdate -= Update;
            ExerussusLoopHelper.OnUpdate += Update;
        }

        public static ActionBuilding.Builder CreateJob(Action action)
        {
            return ActionBuilding.Builder.Create(action);
        }

        // public static FuncBuilding<T>.Builder CreateJob<T>(Func<T> action)
        // {
        //     return FuncBuilding<T>.Builder.Create(action);
        // }

        private static void Update()
        {
            Time = UnityEngine.Time.time;
            ActionBuilding.UpdateActionBuilding();
            _funcBuildingUpdate?.Invoke();
        }
        
        private static void Dispose()
        {
            ExerussusLoopHelper.OnUpdate -= Update;
            _cts.Cancel();
            _cts.Dispose();
            _cts = new();
        }
    }
}