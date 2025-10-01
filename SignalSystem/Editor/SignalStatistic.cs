
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

#if UNITY_EDITOR

namespace Exerussus._1Extensions.SignalSystem.Editor
{
    
    [Serializable]
    internal class SignalStatistic
    {
        public SignalStatistic(Signal signal)
        {
            Signal = signal;
        }

        public Signal Signal { get; }
        [ShowInInspector] public Dictionary<Type, SubCounter> Subs = new();
    }

    [Serializable]
    internal class SubCounter
    {
        public SubCounter(Type type)
        {
            Type = type;
        }

        public Type Type;
        public int Count;
        public float LastUpdateTime;
    }
}

#endif