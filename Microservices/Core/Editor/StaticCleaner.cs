
#if UNITY_EDITOR

    namespace Exerussus._1Extensions.MicroserviceFeature.Core.Editor
    {
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
                    Microservices.UnregisterAll();
                }
            }
        }
    }
#endif