
using System.Collections.Concurrent;
using System.Collections.Generic;

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
    }
}