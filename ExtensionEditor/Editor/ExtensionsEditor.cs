#if UNITY_EDITOR

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Exerussus._1Extensions.ExtensionEditor.Editor
{
    public class ExtensionsEditor : OdinEditorWindow
    {
        [MenuItem("Tools/Exerussus/1Extensions Settings")]
        private static void OpenWindow()
        {
            GetWindow<ExtensionsEditor>("1Extensions Settings");
        }
        
        [Button]
        public void InstallNuGet()
        {
            NuGetForUnityAutoInstaller.InstallNuGetForUnity();
        }
    }
}

#endif