using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class JobQoL
    {
        private static JobHandler _jobHandler;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            _jobHandler = new JobHandler("", logLevel:JobHandler.LogLevel.ErrorsOnly);
            Debug.Log("JobQoL initialized");
            ExerussusCore.OnUpdate += _jobHandler.Update;
        }

        public static void AddJob(Action action, string comment, float delay = 0)
        {
            _jobHandler.AddJob(action, comment, delay);
        }

        public static async Task AddJobAsync(Action action, string comment, float delay = 0, int timeoutMs = 10000)
        {
            await _jobHandler.AddJobAsync(action, comment, delay, timeoutMs);
        }
    }
}