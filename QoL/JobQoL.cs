﻿using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class JobQoL
    {
        private static JobHandler _jobHandler;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _jobHandler ??= new JobHandler("", logLevel: JobHandler.LogLevel.ErrorsOnly);
            ExerussusCore.RemoveOnUpdate(_jobHandler.Update);
            ExerussusCore.AddOnUpdate(_jobHandler.Update);
        }

        public static void AddJob(Action action, string comment, float delay = 0)
        {
            _jobHandler.AddJob(action, comment, delay);
        }

        public static void CreateJob(this Action action, string comment, float delay = 0)
        {
            _jobHandler.AddJob(action, comment, delay);
        }

        public static async Task AddJobAsync(Action action, string comment, float delay = 0, int timeoutMs = 10000)
        {
            await _jobHandler.AddJobAsync(action, comment, delay, timeoutMs);
        }

        public static async Task CreateJobAsync(this Action action, string comment, float delay = 0, int timeoutMs = 10000)
        {
            await _jobHandler.AddJobAsync(action, comment, delay, timeoutMs);
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