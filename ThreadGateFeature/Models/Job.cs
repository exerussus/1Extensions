using System;
using System.Collections.Concurrent;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        private static readonly ConcurrentQueue<Job> Jobs = new();
        
        public class Job
        {
            private Job() { }

            public static Job Create(int id)
            {
                if (!Jobs.TryDequeue(out var job)) job = new();
                job.Id = id;
                return job;
            }

            public static void Release(Job job)
            {
                job.Id = 0;
                job.EndTime = 0;
                job.IsProtected = false;
                Jobs.Enqueue(job);
            }

            public int Id;
            public float EndTime;
            public bool IsProtected;
            public Action Action;
        }
    }
}