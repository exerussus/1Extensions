
using System;
using System.IO;
using Exerussus._1Extensions.ProjectLoader.Loaders;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Exerussus._1Extensions
{
    public static class ConfigLoader
    {
        private const string ConfigFolder = "Configs";
        private const string AssetsFolder = "Assets";
        
        public static T TryGetConfigIfNull<T>(ref T component, string parentFolder)
            where T : ScriptableObject
        {
            if (component == null) component = GetOrCreate<T>(parentFolder);

            return component;
        }
        
        public static T TrySetDataIfNull<T>(ref T component)
            where T : ScriptableObject
        {
            if (component == null)
            {
                if (Application.isPlaying)
                {
                    var configHub = GetConfigHub();
                    configHub.RefreshConfigs();
                    component = configHub.GetConfig<T>();
                }
                else
                {
                    component = Get<T>(typeof(T).Name);
                }
            }

            return component;
        }
        
        public static T GetOrCreate<T>(string parentFolder, string configsFolder = ConfigFolder) where T : ScriptableObject
        {
            T asset = null;

#if UNITY_EDITOR
            
            var assetPath = Path.Combine(AssetsFolder, configsFolder, parentFolder, $"{typeof(T).Name}.asset");
            asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            
            if (asset == null)
            {
                CreateAssetFile<T>(parentFolder, configsFolder);
                asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            
            var configHub = GetConfigHub();
            configHub.RefreshConfigs();
            
#endif
            return asset;
        }

        public static T Get<T>(string parentFolder, string configsFolder = ConfigFolder) where T : ScriptableObject
        {
            T asset = null;
            
#if UNITY_EDITOR
            
            var assetPath = Path.Combine(AssetsFolder, configsFolder, parentFolder, $"{typeof(T).Name}.asset");
            asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
#endif
            return asset;
        }

        public static void CreateAssetFile<T>(string parentFolder, string configsFolder = "Configs") where T : ScriptableObject
        {
            
#if UNITY_EDITOR
            var newConfig = ScriptableObject.CreateInstance<T>();
            var assetName = typeof(T).Name;

            var folderPath = Path.Combine("Assets", configsFolder, parentFolder);

            if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", configsFolder))) CreateFolder("Assets", configsFolder);
    
            if (!AssetDatabase.IsValidFolder(folderPath)) CreateFolder(Path.Combine("Assets", configsFolder), parentFolder);
            
            var assetPathAndName = Path.Combine(folderPath, $"{assetName}.asset");

            if (File.Exists(assetPathAndName)) return;
            
            try
            {
                AssetDatabase.CreateAsset(newConfig, assetPathAndName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
#endif
        }

        public static ConfigHub GetConfigHub()
        {
            ConfigHub configHub;
            
            
#if UNITY_EDITOR
            
            var assetName = typeof(ConfigHub).Name;
            var folderPath = Path.Combine("Assets", "Resources");
            var assetPathAndName = Path.Combine(folderPath, $"{assetName}.asset");
            configHub = AssetDatabase.LoadAssetAtPath<ConfigHub>(assetPathAndName);
            
            if (configHub == null)
            {
                Debug.Log($"ConfigHub isd null");
                try
                {
                    var newConfig = ScriptableObject.CreateInstance<ConfigHub>();


                    if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", "Resources")))
                        CreateFolder("Assets/", "Resources");
                    
                    AssetDatabase.CreateAsset(newConfig, assetPathAndName);
                    AssetDatabase.SaveAssets();
                    configHub = newConfig;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    throw;
                }
            }
#else
            
            configHub = Resources.Load<ConfigHub>("ConfigHub"); 
#endif

            return configHub;
        }
        
        private static void CreateFolder(string parent, string target)
        {
#if UNITY_EDITOR
            AssetDatabase.CreateFolder(parent.TrimEnd('/'), target.TrimEnd('/'));
#endif
        }
    }
}
