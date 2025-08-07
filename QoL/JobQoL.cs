using System;
using System.Threading.Tasks;
using Exerussus._1Extensions.LoopFeature;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class JobQoL
    {
        private static JobHandler _jobHandler;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _jobHandler ??= new JobHandler (logLevel: JobHandler.LogLevel.Errors);
            ExerussusLoopHelper.OnUpdate -= _jobHandler.Update;
            ExerussusLoopHelper.OnUpdate += _jobHandler.Update;
        }

        public static void AddJob(Action action, float delay = 0)
        {
            _jobHandler.AddJob(action, delay);
        }

        public static void CreateJob(this Action action, float delay = 0)
        {
            _jobHandler.AddJob(action, delay);
        }

        public static async Task AddJobAsync(Action action, float delay = 0, int timeoutMs = 10000)
        {
            await _jobHandler.AddJobAsync(action, delay, timeoutMs);
        }

        public static async Task CreateJobAsync(this Action action, float delay = 0, int timeoutMs = 10000)
        {
            await _jobHandler.AddJobAsync(action, delay, timeoutMs);
        }

#if UNITY_EDITOR

        [UnityEditor.InitializeOnLoad]
        public static class StaticCleaner
        {
            static StaticCleaner()
            {
                UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }

            private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    _jobHandler = null;
                }
            }
        }
#endif
    }
}