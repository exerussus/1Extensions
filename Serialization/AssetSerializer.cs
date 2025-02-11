using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    public static class AssetSerializer
    {
        public static async Task SavePersistentAsync<T>(T data, string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var resultPath = path;

            var directory = Path.GetDirectoryName(resultPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);

            await using StreamWriter writer = new StreamWriter(resultPath);
            await writer.WriteAsync(json);
        }
        
        public static async Task<(bool result, T asset)> LoadPersistantAsync<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.persistentDataPath, saveName, fileName);
            var resultPath = path;

            if (!File.Exists(resultPath))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                using (StreamReader reader = new StreamReader(resultPath))
                {
                    string json = await reader.ReadToEndAsync();
                    T asset = JsonUtility.FromJson<T>(json);
                    return (true, asset);
                }
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
            var resultPath = path;

            var directory = Path.GetDirectoryName(resultPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data, true);

            await using StreamWriter writer = new StreamWriter(resultPath);
            await writer.WriteAsync(json);
        }
        
        public static async Task<(bool result, T asset)> LoadStreamingAsync<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(Application.streamingAssetsPath, saveName, fileName);
            var resultPath = path;

            if (!File.Exists(resultPath))
            {
                Debug.LogWarning($"Файл {fileName} не найден в файлах сохранения {saveName}.");
                return (false, default);
            }

            try
            {
                using (StreamReader reader = new StreamReader(resultPath))
                {
                    string json = await reader.ReadToEndAsync();
                    T asset = JsonUtility.FromJson<T>(json);
                    return (true, asset);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, default);
            }
        }
    }
}