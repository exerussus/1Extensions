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

        public static async Task SavePersistentAsync(object data, string saveName)
        {
            var fileName = data.GetType().Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            await File.WriteAllTextAsync(path, json);
        }
        
        public static async Task SavePersistentAsync(object data, string saveName, string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            await File.WriteAllTextAsync(path, json);
        }
        
        public static async Task<(bool result, T asset)> LoadPersistentAsync<T>(T data, string saveName)
        {
            var result = await LoadPersistentAsync(typeof(T), saveName);
            return (result.result, (T)result.asset);
        }
        
        public static async Task<(bool result, T asset)> LoadPersistentAsync<T>(string saveName)
        {
            var result = await LoadPersistentAsync(typeof(T), saveName);
            return (result.result, (T)result.asset);
        }
        
        public static async Task<(bool result, T asset)> LoadPersistentAsync<T>(string saveName, string fileName)
        {
            var result = await LoadPersistentAsync(typeof(T), saveName, fileName);
            return (result.result, (T)result.asset);
        }
        
        public static async Task<(bool result, object asset)> LoadPersistentAsync(Type type, string saveName)
        {
            var fileName = type.Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                string json = await File.ReadAllTextAsync(path);
                object asset = JsonUtility.FromJson(json, type);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, null);
            }
        }
        
        public static async Task<(bool result, object asset)> LoadPersistentAsync(Type type, string saveName, string fileName)
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
                object asset = JsonUtility.FromJson(json, type);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, null);
            }
        }
        
        public static void SavePersistent(object data, string saveName)
        {
            var fileName = data.GetType().Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
        
        public static bool TryLoadAndSetPersistent<T>(ref T data, string saveName)
        {
            var result = LoadPersistent<T>(saveName, data.GetType().Name);
            if (result.result) data = result.asset;
            return result.result;
        }
        
        public static bool TryLoadAndSetPersistent<T>(ref T data, string saveName, string fileName)
        {
            var result = LoadPersistent<T>(saveName, fileName);
            if (result.result) data = result.asset;
            return result.result;
        }

        public static (bool result, T asset) LoadPersistent<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            return LoadPersistent<T>(saveName, fileName);
        }

        public static (bool result, T asset) LoadPersistent<T>(string saveName, string fileName)
        {
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
        
        public static void SaveStreaming(object data, string saveName)
        {
            var fileName = data.GetType().Name;
            var path = Path.Combine(Application.streamingAssetsPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
        
        public static (bool result, T asset) LoadStreaming<T>(string saveName)
        {
            var result = LoadStreaming(typeof(T), saveName);
            return (result.result, (T)result.asset);
        }
        
        public static (bool result, object asset) LoadStreaming(Type type, string saveName)
        {
            var fileName = type.Name;
            var path = Path.Combine(Application.streamingAssetsPath, saveName, fileName);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                string json = File.ReadAllText(path);
                object asset = JsonUtility.FromJson(json, type);
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
