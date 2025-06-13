
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
            Trace trace;
            lock (Traces) Traces.TryDequeue(out trace);
            if (trace == null) trace = new Trace();
            
            trace.Prefix = prefix;
            Debug.Log($"{trace.Prefix} | Tracing started. | {Time.realtimeSinceStartup}");
            return trace;
        }

        public static Trace StartIf(bool isEnabled, string prefix = Tracer.DefaultPrefix)
        {
            Trace trace;
            lock (Traces) Traces.TryDequeue(out trace);
            if (trace == null) trace = new Trace();
            
            trace.Prefix = prefix;
            trace.BlockLogs = !isEnabled;
            if (!trace.BlockLogs) Debug.Log($"{trace.Prefix} | Tracing started. | {Time.realtimeSinceStartup}");
            return trace;
        }

        public static Trace Start(UnityEngine.Object context, string prefix = Tracer.DefaultPrefix)
        {
            Trace trace;
            lock (Traces) Traces.TryDequeue(out trace);
            if (trace == null) trace = new Trace();
            
            trace.Prefix = prefix;
            Debug.Log($"{trace.Prefix} | Tracing started. | {Time.realtimeSinceStartup}", context);
            return trace;
        }

        public static Trace StartIf(UnityEngine.Object context, bool isEnabled, string prefix = Tracer.DefaultPrefix)
        {
            Trace trace;
            lock (Traces) Traces.TryDequeue(out trace);
            if (trace == null) trace = new Trace();
            
            trace.Prefix = prefix;
            trace.BlockLogs = !isEnabled;
            if (!trace.BlockLogs) Debug.Log($"{trace.Prefix} | Tracing started. | {Time.realtimeSinceStartup}", context);
            return trace;
        }

        public static void Ping(int message)
        {
            Debug.Log($"{DefaultPrefix}{message}");
        }

        public static void Ping(int message, UnityEngine.Object context)
        {
            Debug.Log($"{DefaultPrefix}{message}", context);
        }

        public static void Ping(string message)
        {
            Debug.Log($"{DefaultPrefix}{message}");
        }

        public static void Ping(string message, UnityEngine.Object context)
        {
            Debug.Log($"{DefaultPrefix}{message}", context);
        }

        public class Trace : IDisposable
        {
            public string Prefix;
            public bool BlockLogs;
            private int _count;
            private bool _disableLog;
            private bool _isEnded;

            public Trace Ping(string message = "")
            {
                if (_disableLog || BlockLogs) return this;
                _count++;
                Debug.Log($"{Prefix} | {_count}. {message} | {Time.realtimeSinceStartup}");
                return this;
            }

            public Trace SetEnableLogs(bool isEnabled)
            {
                _disableLog = !isEnabled;
                return this;
            }

            public void End()
            {
                _isEnded = true;
                if (!BlockLogs) Debug.Log($"{Prefix} | Tracing end. | {Time.realtimeSinceStartup}");
                Clear();
            }
        
            public void Dispose()
            {
                if (!_isEnded) Debug.LogWarning($"{Prefix} | Tracing not ended. | {Time.realtimeSinceStartup}");
                Clear();
            }

            private void Clear()
            {
                _count = 0;
                _disableLog = false;
                BlockLogs = false;
                lock (Traces) Traces.Enqueue(this);
            }
        }
    }
}