using System;
using UnityEngine;

namespace Exerussus._1Extensions
{
    internal class ExerussusCore : MonoBehaviour
    {
        private static ExerussusCore _instance;
        public static event Action OnUpdate;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            if (_instance != null) return;

            var go = new GameObject("[Exerussus Core]");
            go.AddComponent<ExerussusCore>();
            DontDestroyOnLoad(go);
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }
        
        
#if UNITY_EDITOR

        [UnityEditor.InitializeOnLoad]
        private static class StaticCleaner
        {
            static StaticCleaner()
            {
                UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            }

            private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    OnUpdate = null;
                }
            }
        }
        
#endif
    }
}