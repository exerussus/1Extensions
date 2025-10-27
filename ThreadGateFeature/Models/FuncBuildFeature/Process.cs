using System;
using System.Collections.Generic;
using Exerussus._1Extensions.Scripts.Extensions;
using UnityEngine;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        public static partial class FuncBuilding<T>
        {
            private static readonly HashSet<int> ToRelease = new();
            private static readonly object CreateLock = new();
            private static bool _isUpdating;
            
            private static void UpdateFuncBuilding()
            {
                UpdateReleasing();
                UpdateCreating();
                UpdateWaiting();

                lock (CreateLock)
                {
                    if (ToCreate.Count == 0 && ToWait.Count == 0 && ToRelease.Count == 0)
                    {
                        ThreadGate._funcBuildingUpdate -= UpdateFuncBuilding;
                        _isUpdating = false;
                    }
                }
            }

            private static void UpdateCreating()
            {
                lock (CreateLock)
                {
                    foreach (var job in FuncBuilding<T>.ToCreate.Values)
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
                        if (job.IsReturnable) Results[job.Id] = job.Action.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                else
                {
                    if (job.IsReturnable) Results[job.Id] = job.Action.Invoke();
                }
            }
        
            private static void CreateJob(FuncBuilding<T>.Buffer actionBuffer, int jobId)
            {
                var job = FuncBuilding<T>.Job.Create(jobId);
                job.EndTime = Time + actionBuffer.Delay;
                job.Action = actionBuffer.Action;
                job.IsProtected = actionBuffer.IsProtected;
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

            private static void SetReturnable(int jobId)
            {
                lock (ToCreate)
                {
                    if (!ToCreate.TryGetValue(jobId, out var job) && !ToWait.TryGetValue(jobId, out job))
                    {
                        Debug.LogError($"SetReturnable error: job {jobId} not found");
                        return;
                    }

                    job.IsReturnable = true;
                }
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