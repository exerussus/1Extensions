using System;
using System.Collections.Generic;
using Exerussus._1Extensions.Abstractions;
using Exerussus._1Extensions.Scripts.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Exerussus._1Extensions.SmallFeatures
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private StartType startType = StartType.Awake;
        
        public bool destroyGameObjectOnDone = true;
        public bool enableWarnings = true;
        public bool enableErrors = true;
        public List<BootstrapStage> initializeQueue = new();
        
        private readonly List<IInitializable> _current = new();
        private readonly HashSet<IInitializable> _tempNewInits = new();
        private readonly HashSet<IInitializable> _timeoutObjects = new();
        private readonly HashSet<IInitializable> _totalObjects = new();

        private readonly object _lock = new();
        private bool _isStarted;
        private float _awaitTimeout;
        private int _initCount = 0;
        
        public event Action OnAllInitialized;
        public event Action<int> OnStarted;
        public event Action<(int objectsDone, int totalObjects, IInitializable initializable)> OnInitialized;
        public event Action<Exception> OnError;
        public event Action<IInitializable> OnTimeout;

        public void Initialize()
        {
            Run();
        }
        
        private void Awake()
        {
            if (startType != StartType.Awake) return; 
            Run();
        }

        private void Start()
        {
            if (startType != StartType.Start) return;
            Run();
        }

        private void Run()
        {
            lock (_lock)
            {
                if (_isStarted) return;
                _isStarted = true;
                if (TryDestroy()) return;

                foreach (var bootstrapStage in initializeQueue)
                {
                    if (bootstrapStage.objects is not { Count: > 0 }) continue;
                    foreach (var obj in bootstrapStage.objects)
                    {
                        if (obj is IInitializable initializable) _totalObjects.Add(initializable);
                    }
                }
                
                OnStarted?.Invoke(_totalObjects.Count);
                Next();
            }
        }

        private void Update()
        {
            var hasPending = false;
            
            for (var index = _current.Count - 1; index >= 0; index--)
            {
                var initializable = _current[index];
                
                if (initializable == null)
                {
                    _current.RemoveAt(index);
                    continue;
                }
                
                if (!initializable.IsInitialized)
                {
                    if (Time.time > _awaitTimeout && _timeoutObjects.Add(initializable))
                    {
                        if (enableWarnings) Debug.LogWarning($"[Bootstrapper] Timeout: {initializable}");
                        OnTimeout?.Invoke(initializable);
                    }

                    hasPending = true;
                    continue;
                }

                _initCount++;
                OnInitialized?.Invoke((_initCount, _totalObjects.Count, initializable));
                
                _timeoutObjects.Remove(initializable);
                _current.RemoveAt(index);
            }
            
            if (hasPending) return;

            _current.Clear();

            if (TryDestroy()) return;
            Next();
        }

        private void Next()
        {
            if (initializeQueue.Count == 0) return;

            var stage = initializeQueue.PopFirst();
            if (stage?.objects == null) return;

            _awaitTimeout = stage.customTimeout > 0 ? Time.time + stage.customTimeout : float.MaxValue;

            foreach (var obj in stage.objects)
            {
                if (obj is IInitializable initializable) _tempNewInits.Add(initializable);
            }

            foreach (var initializable in _tempNewInits) _current.Add(initializable);
            _tempNewInits.Clear();
            
            foreach (var initializable in _current)
            {
                try
                {
                    initializable.Initialize();
                }
                catch (Exception e)
                {
                    if (enableErrors) Debug.LogError(e);
                    OnError?.Invoke(e);
                }
            }
        }

        private bool TryDestroy()
        {
            if (initializeQueue.Count == 0)
            {
                OnAllInitialized?.Invoke();
                Debug.Log("[Bootstrapper] All initialized.");
                if (destroyGameObjectOnDone) Destroy(gameObject);
                else Destroy(this);
                return true;
            }

            return false;
        }
        [Serializable]
        public class BootstrapStage
        {
            [FoldoutGroup("$name")] public string name;
            [FoldoutGroup("$name")] public List<Object> objects = new();
            [FoldoutGroup("$name")] public float customTimeout;
        }

        public enum StartType
        {
            None,
            Awake,
            Start,
        }
    }
}
