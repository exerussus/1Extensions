using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    public static class AssetSerializer
    {
        public static void DeletePersistentFolderContents(string saveName, bool debugLog = false)
        {
            var path = Path.Combine(Application.persistentDataPath, saveName);
        
            if (!Directory.Exists(path))
            {
                if (debugLog) Debug.LogWarning($"Папка '{path}' не существует.");
                return;
            }

            try
            {
                foreach (var file in Directory.GetFiles(path)) File.Delete(file);
                foreach (var directory in Directory.GetDirectories(path)) Directory.Delete(directory, true);
                
                if (debugLog) Debug.Log($"Все содержимое папки '{path}' удалено.");
            }
            catch (IOException e)
            {
                Debug.LogError($"Ошибка при удалении файлов в папке '{path}': {e.Message}");
            }
        }
        
        public static async Task SavePersistentAsync<T>(T data, string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            await File.WriteAllTextAsync(path, json);
        }
        
        public static async Task SavePersistentAsync<T>(T data, string saveName, string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            await File.WriteAllTextAsync(path, json);
        }
        
        public static async Task<(bool result, T asset)> LoadPersistentAsync<T>(string saveName, string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                string json = await File.ReadAllTextAsync(path);
                T asset = JsonUtility.FromJson<T>(json);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, default);
            }
        }
        
        public static async Task<(bool result, T asset)> LoadPersistentAsync<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                string json = await File.ReadAllTextAsync(path);
                T asset = JsonUtility.FromJson<T>(json);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, default);
            }
        }
        
        public static void SavePersistent<T>(T data, string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
        
        public static (bool result, T asset) LoadPersistent<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                string json = File.ReadAllText(path);
                T asset = JsonUtility.FromJson<T>(json);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, default);
            }
        }
        
        public static async Task SaveStreamingAsync<T>(T data, string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.streamingAssetsPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            await File.WriteAllTextAsync(path, json);
        }
        
        public static async Task<(bool result, T asset)> LoadStreamingAsync<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.streamingAssetsPath, saveName, fileName);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                string json = await File.ReadAllTextAsync(path);
                T asset = JsonUtility.FromJson<T>(json);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, default);
            }
        }
        
        public static void SaveStreaming<T>(T data, string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.streamingAssetsPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
        
        public static (bool result, T asset) LoadStreaming<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.streamingAssetsPath, saveName, fileName);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                string json = File.ReadAllText(path);
                T asset = JsonUtility.FromJson<T>(json);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, default);
            }
        }
    }
}
