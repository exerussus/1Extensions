
using System;
using System.Collections.Generic;
using Exerussus._1Extensions.LoopFeature;
using Exerussus._1Extensions.Scripts.Extensions;
using UnityEngine;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        private static readonly Dictionary<int, Job> ToCreate = new();
        private static readonly Dictionary<int, Job> ToWait = new();
        private static readonly HashSet<int> ToRelease = new();
        private static readonly object CreateLock = new();
        
        private static float _time = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            ExerussusLoopHelper.OnUpdate -= Update;
            ExerussusLoopHelper.OnUpdate += Update;
        }
        
        private static void Update()
        {
            _time = Time.time;
            UpdateCreating();
            UpdateWaiting();
            UpdateReleasing();
        }

        private static void UpdateCreating()
        {
            lock (CreateLock)
            {
                foreach (var job in ToCreate.Values)
                {
                    ToWait[job.Id] = job;
                }
                
                ToCreate.Clear();
            }
        }

        private static void UpdateWaiting()
        {
            foreach (var job in ToWait.Values)
            {
                if (ToRelease.Contains(job.Id)) continue;

                if (job.EndTime < _time)
                {
                    ExecuteJob(job);
                    ToRelease.Add(job.Id);
                }
            }
        }

        private static void UpdateReleasing()
        {
            foreach (var jobId in ToRelease)
            {
                var job = ToWait.Pop(jobId);
                Job.Release(job);
            }
            
            ToRelease.Clear();
        }

        private static void ExecuteJob(Job job)
        {
            if (job.IsProtected)
            {
                try
                {
                    job.Action.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                job.Action.Invoke();
            }
        }

        public static Builder CreateJob(Action action)
        {
            return Builder.Create(action);
        }
        
        private static void CreateJob(Buffer buffer, int jobId)
        {
            var job = Job.Create(jobId);
            job.EndTime = _time + buffer.Delay;
            job.Action = buffer.Action;
            job.IsProtected = buffer.IsProtected;
            lock (CreateLock) ToCreate.Add(job.Id, job);
        }

        private static bool TryCancel(int jobId)
        {
            if (!ToWait.ContainsKey(jobId)) return false;
            ToRelease.Add(jobId);
            return true;
        }

        private static void Cancel(int jobId)
        {
            if (ToWait.ContainsKey(jobId)) ToRelease.Add(jobId);
        }

        private static bool IsValid(int jobId)
        {
            return jobId != 0 && jobId <= _freeJobIndex;
        }

        private static bool IsDone(int jobId)
        {
            return IsValid(jobId) && !ToWait.ContainsKey(jobId);
        }
    }
}