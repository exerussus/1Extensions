
using System.Collections.Concurrent;
using System.Collections.Generic;
using Exerussus._1Extensions.LoopFeature;

namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public static partial class DelayedAction
    {
        private static readonly ConcurrentQueue<Operation> Pool = new();
        private static readonly Dictionary<int, Operation> DictInWork = new();
        private static readonly Dictionary<int, Operation> ToCreate = new();
        private static readonly HashSet<int> ToRelease = new();
        private static float _time;
        private static int _nextId = 1;
        private static readonly object IdLock = new();
        private static bool _isInitialized = false;

        private static void TryInitialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            ExerussusLoopHelper.OnUpdate -= Update;
            ExerussusLoopHelper.OnUpdate += Update;
        }
    }
}