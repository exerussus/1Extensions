
using System;
using System.IO;
using Plugins.Exerussus._1Extensions.ProjectLoader.Loaders;
using UnityEngine;
using UnityEditor;

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
        
        public static T GetOrCreate<T>(string parentFolder, string configsFolder = ConfigFolder) where T : ScriptableObject
        {
            
            var assetPath = Path.Combine(AssetsFolder, configsFolder, parentFolder, $"{typeof(T).Name}.asset");
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            
            if (asset == null)
            {
                CreateAssetFile<T>(parentFolder, configsFolder);
                asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            
            var configHub = GetConfigHub();
            configHub.RefreshConfigs();
            
            return asset;
        }

        public static T Get<T>(string parentFolder, string configsFolder = ConfigFolder) where T : ScriptableObject
        {
            var assetPath = Path.Combine(AssetsFolder, configsFolder, parentFolder, $"{typeof(T).Name}.asset");
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public static void CreateAssetFile<T>(string parentFolder, string configsFolder = "Configs") where T : ScriptableObject
        {
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
                throw;
            }
        }

        public static ConfigHub GetConfigHub()
        {
            var configHub = Resources.Load<ConfigHub>("ConfigHub"); 
            
            if (configHub == null) 
            {
                var newConfig = ScriptableObject.CreateInstance<ConfigHub>();
                var assetName = typeof(ConfigHub).Name;

                var folderPath = Path.Combine("Assets", "Resources"); 
        
                if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", "Resources"))) CreateFolder("Assets/", "Resources");  
                
                var assetPathAndName = Path.Combine(folderPath, $"{assetName}.asset");

                AssetDatabase.CreateAsset(newConfig, assetPathAndName);
                AssetDatabase.SaveAssets(); 
            }

            return Resources.Load<ConfigHub>("ConfigHub");
        }
        
        private static void CreateFolder(string parent, string target)
        {
            AssetDatabase.CreateFolder(parent.TrimEnd('/'), target.TrimEnd('/'));
        }
    }
}
