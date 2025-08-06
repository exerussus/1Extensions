
namespace Exerussus._1Extensions.DelayedActionsFeature
{
    public static partial class DelayedAction
    {
        
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
                    Pool.Clear();
                    DictInWork.Clear();
                    ToCreate.Clear();
                    ToRelease.Clear();
                    _time = 0;
                    _nextId = 1;
                }
            }
        }
#endif
    }
}