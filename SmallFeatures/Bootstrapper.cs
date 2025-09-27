using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Exerussus._1Extensions.Abstractions;
using Exerussus._1Extensions.Scripts.Extensions;
using Exerussus._1Extensions.ThreadGateFeature;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Exerussus._1Extensions.SmallFeatures
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private StartType startType = StartType.Awake;

        public bool safeMode = true;
        public bool destroyGameObjectOnDone = true;
        public bool enableErrors = true;
        public List<BootstrapStage.Settings> initializeQueue = new();
        
        [ReadOnly, FoldoutGroup("DEBUG")] public List<BootstrapStage.Runtime> initializeQueueProcess = new();
        [ReadOnly, FoldoutGroup("DEBUG"), ShowInInspector] private List<InitializingObject> _current = new();

        private readonly object _lock = new();
        private bool _isStarted;

        public event Action OnStarted;
        public event Action OnAllInitialized;
        public event Action<Exception> OnError;

        public void Initialize()
        {
            Run().Forget();
        }
        
        private void Awake()
        {
            if (startType != StartType.Awake) return; 
            Run().Forget();
        }

        private void Start()
        {
            if (startType != StartType.Start) return;
            Run().Forget();
        }
        
        public virtual void OnPreInitialize() {}
        public virtual void OnPostInitialize() {}

        private async UniTask Run()
        {
            lock (_lock)
            {
                if (_isStarted) return;
                _isStarted = true;
            }

            await ThreadGate.CreateJob(OnPreInitialize).Run().AsUniTask();
            
            initializeQueueProcess.Clear();
            
            BootstrapStage.CreateSettings(initializeQueue, initializeQueueProcess);

            await ThreadGate.CreateJob(() => OnStarted?.Invoke()).Run().AsUniTask();
            
            var destroyResult = await TryDestroy();
            
            if (destroyResult) return;
            await Next();
            await TryDestroy();
        }

        private async UniTask Next()
        {
            while (initializeQueueProcess.Count != 0)
            {
                var stage = initializeQueueProcess.PopFirst();
                if (stage?.Objects == null)
                {
                    _current.Clear();
                    continue;
                }

                foreach (var initializable in stage.Objects) _current.Add(initializable);

                if (_current.Count == 0) continue;

                var tasks = new UniTask[_current.Count];

                for (var index = 0; index < _current.Count; index++)
                {
                    var indexOfCurrent = index;
                    var initializingObject = _current[indexOfCurrent];
                    await ThreadGate.CreateJob(() => tasks[indexOfCurrent] = initializingObject.Task.Invoke()).Run().AsUniTask();
                }

                if (safeMode)
                {
                    try
                    {
                        await UniTask.WhenAll(tasks);
                    }
                    catch (Exception e)
                    {
                        if (enableErrors) Debug.LogError(e);
                        await ThreadGate.CreateJob(() => OnError?.Invoke(e)).Run().AsUniTask();
                    }
                }
                else
                {
                    await UniTask.WhenAll(tasks);
                }
                
                _current.Clear();
            }
        }

        private async UniTask<bool> TryDestroy()
        {
            if (initializeQueueProcess.Count == 0)
            {
                await ThreadGate.CreateJob(() => OnAllInitialized?.Invoke()).Run().AsUniTask();
                OnStarted = null;
                OnAllInitialized = null;
                OnError = null;
                await ThreadGate.CreateJob(OnPostInitialize).Run().AsUniTask();
                Debug.Log("[Bootstrapper] All initialized.");
                ThreadGate.CreateJob(() =>
                {
                    if (destroyGameObjectOnDone) Destroy(gameObject);
                    else Destroy(this);
                }).WithDelay(3).Run();
                return true;
            }

            return false;
        }
        
        public static class BootstrapStage
        {
            public class Runtime
            {
                public Runtime(Settings settings)
                {
                    if (settings == null) return;
                    
                    foreach (var obj in settings.objects)
                    {
                        if (obj == null) continue;
                        if (obj is IInitializable initializable) Objects.Add(new InitializingObject(initializable));
                        else if (obj is IInitializableAsync initializableAsync) Objects.Add(new InitializingObject(initializableAsync));
                    }
                }

                public readonly List<InitializingObject> Objects = new();
            }

            [Serializable]
            public class Settings
            {
                [FoldoutGroup("$name")] public string name;
                [FoldoutGroup("$name")] public List<Object> objects = new();
            }

            public static void CreateSettings(List<Settings> settings, List<Runtime> initializeQueueProcess)
            {
                if (settings == null || initializeQueueProcess == null) return;
                foreach (var setting in settings)
                {
                    if (setting == null) continue;
                    initializeQueueProcess.Add(new Runtime(setting));
                }
            }
        }

        public enum StartType
        {
            Manual,
            Awake,
            Start,
        }

        public class InitializingObject
        {
            public InitializingObject(IInitializableAsync initializable)
            {
                Task = initializable.Initialize;
                Type = initializable.GetType();
            }

            public InitializingObject(IInitializable initializable)
            {
                Task = ConvertToTask(initializable.Initialize);
                Type = initializable.GetType();
            }

            public readonly Func<UniTask> Task;
            public readonly Type Type;

            private static Func<UniTask> ConvertToTask(Action action)
            {
                return () =>
                {
                    action();
                    return UniTask.CompletedTask;
                };
            }
        }
    }
}
