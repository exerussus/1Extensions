using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class Tracer
    {
        private const string DefaultPrefix = ">>>>>>>>>>>>>>> ";
        private static string _prefix = DefaultPrefix;
        
        public static void Start(string prefix)
        {
            _prefix = prefix;
            Debug.Log($"{_prefix}Start");
        }
        
        public static void End()
        {
            Debug.Log($"{_prefix}End");
            _prefix = DefaultPrefix;
        }

        public static void Ping(int message)
        {
            Debug.Log($"{_prefix}{message}");
        }

        public static void Ping(string message)
        {
            Debug.Log($"{_prefix}{message}");
        }
    }
}