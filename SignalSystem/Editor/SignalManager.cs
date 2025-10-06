
#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.SignalSystem.Editor
{
    internal static class SignalManager
    {
        public static void RegisterSignal<T>(Signal signal)
        {
            if (!_signals.TryGetValue(signal.UniqId, out var statistic))
            {
                statistic = new SignalStatistic(signal);
                _signals[signal.UniqId] =  statistic;
            }

            var type = typeof(T);

            if (!statistic.Subs.TryGetValue(type, out var subCounter))
            {
                subCounter = new SubCounter(type);
                statistic.Subs[type] = subCounter;
            }

            subCounter.LastUpdateTime = Time.time;
            subCounter.Count++;
        }

        internal static readonly Dictionary<long, SignalStatistic> _signals = new();
        public static long StateHash { get; private set; }
        
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
                    _signals.Clear();
                    StateHash = 0;
                }
            }
        }
    }
}

#endif