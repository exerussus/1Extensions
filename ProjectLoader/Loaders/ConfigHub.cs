
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace Exerussus._1Extensions.ProjectLoader.Loaders
{
    [Preserve]
    public class ConfigHub : ScriptableObject
    {
        [SerializeField] private List<ScriptableObject> configs;

        public List<ScriptableObject> Configs => configs;

        public T GetConfig<T>() where T : ScriptableObject
        {
            foreach (var scriptableObject in configs) if (scriptableObject is T config) return config;
            
            return null;
        }
        
        public void RefreshConfigs()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            configs = new List<ScriptableObject>();
            string configsPath = "Assets/Configs"; 

            string[] assetPaths = Directory.GetFiles(configsPath, "*.asset", SearchOption.AllDirectories);

            foreach (var path in assetPaths)
            {
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset != null) configs.Add(asset);
            }
#endif
        }
    }
}