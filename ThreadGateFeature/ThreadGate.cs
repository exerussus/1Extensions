
using System;
using Exerussus._1Extensions.LoopFeature;
using UnityEngine;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        internal static float Time = 0;
        private static Action _funcBuildingUpdate;
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

        public static FuncBuilding<T>.Builder CreateJob<T>(Func<T> action)
        {
            return FuncBuilding<T>.Builder.Create(action);
        }

        private static void Update()
        {
            Time = UnityEngine.Time.time;
            ActionBuilding.UpdateActionBuilding();
            _funcBuildingUpdate?.Invoke();
        }
    }
}