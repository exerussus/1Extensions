
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Exerussus._1Extensions.Scripts.Extensions;

namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        private static readonly ConcurrentQueue<Buffer> Buffers = new();
        private static readonly Dictionary<int, Buffer> ActiveBuffers = new();
        
        private static void SetDelay(int builderId, float delay)
        {
            if (!ActiveBuffers.TryGetValue(builderId, out var buffer)) return;
            
            buffer.Delay = delay;
        }
        
        private static void SetProtection(int builderId)
        {
            if (!ActiveBuffers.TryGetValue(builderId, out var buffer)) return;
            
            buffer.IsProtected = true;
        }
        
        private static void SetPreserve(int builderId)
        {
            if (!ActiveBuffers.TryGetValue(builderId, out var buffer)) return;
            
            buffer.IsPreserved = true;
        }

        private static bool GetIsValid(int builderId)
        {
            return ActiveBuffers.ContainsKey(builderId);
        }

        private static void BakeJob(int builderId, int jobId)
        {
            if (!ActiveBuffers.TryGetValue(builderId, out var buffer)) return;

            CreateJob(buffer, jobId);
            
            if (buffer.IsPreserved) return;
            
            buffer.Delay = 0;
            buffer.IsPreserved = false;
            buffer.Action = null;
            ActiveBuffers.Remove(builderId);
            Buffers.Enqueue(buffer);
        }

        private static void Break(int builderId)
        {
            if (!ActiveBuffers.TryPop(builderId, out var buffer)) return;
            
            buffer.Delay = 0;
            buffer.IsPreserved = false;
            buffer.Action = null;
            Buffers.Enqueue(buffer);
        }

        public class Buffer
        {
            private Buffer() { }

            public static void Create(int id, Action action)
            {
                if (!Buffers.TryDequeue(out var buffer)) buffer = new Buffer();
                ActiveBuffers[id] = buffer;
                buffer.Action = action;
            }


            public float Delay;
            public bool IsProtected;
            public bool IsPreserved;
            public Action Action;
        }
    }
}