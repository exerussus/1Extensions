using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Exerussus._1Extensions.SmallFeatures
{
    public static class AssetSaver
    {
        private static async Task SaveAsyncTask<T>(T data, string saveName)
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

        
        private static async Task<(bool result, T asset)> LoadAsyncTask<T>(string saveName)
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
                Debug.LogWarning($"Отсутствует файл {fileName}: {e.Message}");
                return (false, default);
            }
        }
    }
}