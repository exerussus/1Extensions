using System.Threading;

#if UNITY_EDITOR
namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
    {
        private static CancellationTokenSource _cts = new();
        
        public static partial class FuncBuilding<T>
        {
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
                        _cts.Cancel();
                        ToWait.Clear();
                        ToRelease.Clear();
                        ActiveBuffers.Clear();
                        Time = 0;
                        _freeJobIndex = 1;
                        _freeBuilderIndex = 1;
                        _funcBuildingUpdate = null;
                        EditorDispose?.Invoke();
                        EditorDispose = null;
                        _cts = new();
                    }
                }
            }
        }
    }
}
#endif