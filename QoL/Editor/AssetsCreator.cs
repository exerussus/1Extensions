
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Exerussus._1Extensions.QoL.Editor
{
    public static class AssetsCreator
    {
        /// <summary> Создает ScriptableObject по указанному пути с указанным именем, если не существует.
        /// Если по пути не существуют папки - создаёт их. </summary>
        /// <param name="folder">Путь к папку, например "Assets/Configs/ExerussusCenter/Editor".</param>
        /// <param name="fileName">Название файла, например, "SomeConfig".</param>
        /// <typeparam name="T">Тип Scriptable Object, который надо создать по пути.</typeparam>
        /// <returns>Итоговый AssetPath созданного Scriptable Object.</returns>
        public static string CreateScriptableObject<T>(string folder, string fileName) where T : ScriptableObject
        {
            var assetPath = $"{folder}/{fileName}.asset";
            var existingAsset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (existingAsset != null) return assetPath;
            
            CreateFolderIfNotExists("Assets", "Configs");
            CreateFolderIfNotExists("Assets/Configs", "ExerussusCenter");
            CreateFolderIfNotExists("Assets/Configs/ExerussusCenter", "Editor");

            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"{typeof(T).Name} создан по пути: " + assetPath);
            return assetPath;
        }

        private static void CreateFolderIfNotExists(string parent, string folderName)
        {
            var fullPath = $"{parent}/{folderName}";
            if (!AssetDatabase.IsValidFolder(fullPath))
            {
                AssetDatabase.CreateFolder(parent, folderName);
            }
        }
    }
}

#endif