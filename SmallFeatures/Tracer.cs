
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class Tracer
    {
        public const string DefaultPrefix = ">>>>>>>>>>>>>>> ";

        private static readonly Queue<Trace> Traces = new();

        public static Trace Start(string prefix = Tracer.DefaultPrefix)
        {
            if (!Traces.TryDequeue(out var trace)) trace = new Trace();
            
            trace.Prefix = prefix;
            Debug.Log($"{trace.Prefix} | Tracing started.");
            return trace;
        }

        public static void Ping(int message)
        {
            Debug.Log($"{DefaultPrefix}{message}");
        }

        public static void Ping(string message)
        {
            Debug.Log($"{DefaultPrefix}{message}");
        }

        public class Trace : IDisposable
        {
            public string Prefix;
            private int _count;
        
            public Trace Ping(string message = "")
            {
                _count++;
                Debug.Log($"{Prefix} | {_count}. {message}");
                return this;
            }

            public void End()
            {
                Debug.Log($"{Prefix} | Tracing end.");
                _count = 0;
                Traces.Enqueue(this);
            }
        
            public void Dispose()
            {
                End();
            }
        }
    }
}