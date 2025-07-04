using UnityEngine;

namespace Exerussus._1Extensions
{
    public class ExerussusCore : MonoBehaviour
    {
        private static readonly object InstanceLock = new object();
        private static ExerussusCore _instance;
        private static event AwakeEvent OnAwake;
        private static event StartEvent OnStart;
        private static event UpdateEvent OnUpdate;
        
        public static bool IsQuiting
        {
            get
            {
                Init();
                return _instance._isQuiting;
            }
        }
        
        private bool _isQuiting;

        public static void AddOnAwake(AwakeEvent @event)
        {
            Init();
            OnAwake += @event;
        }

        public static void AddOnStart(StartEvent @event)
        {
            Init();
            OnStart += @event;
        }

        public static void AddOnUpdate(UpdateEvent @event)
        {
            Init();
            OnUpdate += @event;
        }

        public static void RemoveOnAwake(AwakeEvent @event)
        {
            OnAwake -= @event;
        }

        public static void RemoveOnStart(StartEvent @event)
        {
            OnStart -= @event;
        }

        public static void RemoveOnUpdate(UpdateEvent @event)
        {
            OnUpdate -= @event;
        }

        public static void Init()
        {
            if (_instance != null) return;

            lock (InstanceLock)
            {
                var go = new GameObject("[Exerussus Core]");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<ExerussusCore>();
                Application.quitting += () => _instance._isQuiting = true; 
                go.SetActive(true);
            }
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
                    OnAwake = null;
                    OnStart = null;
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