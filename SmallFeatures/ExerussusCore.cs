
using UnityEngine;

namespace Exerussus._1Extensions
{
    internal class ExerussusCore : MonoBehaviour
    {
        private static ExerussusCore _instance;
        public static event AwakeEvent OnAwake;
        public static event StartEvent OnStart;
        public static event UpdateEvent OnUpdate;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            if (_instance != null) return;

            var go = new GameObject("[Exerussus Core]");
            go.AddComponent<ExerussusCore>();
            DontDestroyOnLoad(go);
        }
        
        private void Awake()
        {
            OnAwake?.Invoke();
        }

        private void Start()
        {
            OnStart?.Invoke();
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


    public delegate void AwakeEvent();
    public delegate void StartEvent();
    public delegate void UpdateEvent();
}