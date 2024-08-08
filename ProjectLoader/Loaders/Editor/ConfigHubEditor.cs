
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Exerussus._1Extensions.ProjectLoader.Loaders
{

    [CustomEditor(typeof(ConfigHub))]
    public class ConfigHubEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ConfigHub configHub = (ConfigHub)target;

            if (configHub.Configs != null)
            {
                foreach (var config in configHub.Configs)
                {
                    EditorGUILayout.ObjectField(config, typeof(ScriptableObject), false);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No Configs");
            }

            // Кнопка для обновления конфигураций
            if (GUILayout.Button("Refresh Configs"))
            {
                configHub.RefreshConfigs();
            }

            // Отметка, что объект изменен
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}

#endif