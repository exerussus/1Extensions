using System;
using System.Collections.Generic;
using Exerussus._1Extensions.Scripts.Extensions;
using UnityEngine;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        public static partial class ActionBuilding
        {
            private static readonly HashSet<int> ToRelease = new();
            private static readonly object CreateLock = new();
            
            internal static void UpdateActionBuilding()
            {
                Time = UnityEngine.Time.time;
                UpdateReleasing();
                UpdateCreating();
                UpdateWaiting();
            }

            private static void UpdateCreating()
            {
                lock (CreateLock)
                {
                    foreach (var job in ActionBuilding.ToCreate.Values)
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

                    if (job.EndTime < Time)
                    {
                        ToRelease.Add(job.Id);
                        ExecuteJob(job);
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
        
            private static void CreateJob(ActionBuilding.Buffer buffer, int jobId)
            {
                var job = ActionBuilding.Job.Create(jobId);
                job.EndTime = Time + buffer.Delay;
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
                bool hasInCreated;
                lock (CreateLock) hasInCreated = ToCreate.ContainsKey(jobId); 
                return IsValid(jobId) && !ToWait.ContainsKey(jobId) && !hasInCreated;
            }
        }
    }
}