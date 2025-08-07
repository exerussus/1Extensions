
#if UNITY_EDITOR
namespace Exerussus._1Extensions.ThreadGateFeature
{
    public static partial class ThreadGate
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
                    ToWait.Clear();
                    ToRelease.Clear();
                    ActiveBuffers.Clear();
                    _time = 0;
                    _freeJobIndex = 1;
                    _freeBuilderIndex = 1;
                }
            }
        }
    }
}
#endif