using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class Tracer
    {
        private static string _prefix = ">>>>>>>>>>>>>>> ";
        
        public static void Start(string prefix)
        {
            _prefix = prefix;
            Debug.Log($"{_prefix}Start");
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