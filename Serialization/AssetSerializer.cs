using System;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    public static class AssetSerializer
    {
        private static readonly string PersistentDataPath = Application.persistentDataPath;
        private static readonly string StreamingAssetsPath = Application.streamingAssetsPath;

        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// Удаляет всё содержимое указанной папки в PersistentDataPath.
        /// </summary>
        public static void DeletePersistentFolderContents(string saveName, bool debugLog = false)
        {
            var path = Path.Combine(PersistentDataPath, saveName);

            if (!Directory.Exists(path))
            {
                if (debugLog) Debug.LogWarning($"Папка '{path}' не существует.");
                return;
            }

            try
            {
                foreach (var file in Directory.GetFiles(path))
                    File.Delete(file);
                foreach (var directory in Directory.GetDirectories(path))
                    Directory.Delete(directory, true);

                if (debugLog) Debug.Log($"Все содержимое папки '{path}' удалено.");
            }
            catch (IOException e)
            {
                Debug.LogError($"Ошибка при удалении файлов в папке '{path}': {e.Message}");
            }
        }

        /// <summary>
        /// Асинхронно сохраняет объект в PersistentDataPath.
        /// </summary>
        public static async UniTask SavePersistentAsync<T>(T data, string saveName)
        {
            var fileName = data.GetType().Name;
            await SavePersistentAsync(data, saveName, fileName);
        }

        public static async UniTask SavePersistentAsync<T>(T data, string saveName, string fileName)
        {
            var path = Path.Combine(PersistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var json = JsonConvert.SerializeObject(data, JsonSettings);
            await File.WriteAllTextAsync(path, json, Encoding.UTF8);
        }

        public static async UniTask<(bool result, T asset)> LoadPersistentAsync<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            return await LoadPersistentAsync<T>(saveName, fileName);
        }

        public static async UniTask<(bool result, T asset)> LoadPersistentAsync<T>(string saveName, string fileName)
        {
            var path = Path.Combine(PersistentDataPath, saveName, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в сохранениях {saveName}.");
                return (false, default);
            }

            try
            {
                var json = await File.ReadAllTextAsync(path, Encoding.UTF8);
                var asset = JsonConvert.DeserializeObject<T>(json, JsonSettings);
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
            var fileName = data.GetType().Name;
            var path = Path.Combine(PersistentDataPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var json = JsonConvert.SerializeObject(data, JsonSettings);
            File.WriteAllText(path, json, Encoding.UTF8);
        }

        public static bool TryLoadAndSetPersistent<T>(ref T data, string saveName)
        {
            var result = LoadPersistent<T>(saveName);
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
            var path = Path.Combine(PersistentDataPath, saveName, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в сохранениях {saveName}.");
                return (false, default);
            }

            try
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                var asset = JsonConvert.DeserializeObject<T>(json, JsonSettings);
                return (true, asset);
            }
            catch (Exception e)
            {
                Debug.LogError($"Не удалось загрузить файл {fileName}: {e.Message}");
                return (false, default);
            }
        }

        /// <summary>
        /// Асинхронное сохранение данных в StreamingAssets.
        /// </summary>
        public static async UniTask SaveStreamingAsync<T>(T data, string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(StreamingAssetsPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var json = JsonConvert.SerializeObject(data, JsonSettings);
            await File.WriteAllTextAsync(path, json, Encoding.UTF8);
        }

        public static async UniTask<(bool result, T asset)> LoadStreamingAsync<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            var path = Path.Combine(StreamingAssetsPath, saveName, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в StreamingAssets/{saveName}.");
                return (false, default);
            }

            try
            {
                var json = await File.ReadAllTextAsync(path, Encoding.UTF8);
                var asset = JsonConvert.DeserializeObject<T>(json, JsonSettings);
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
            var fileName = data.GetType().Name;
            var path = Path.Combine(StreamingAssetsPath, saveName, fileName);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var json = JsonConvert.SerializeObject(data, JsonSettings);
            File.WriteAllText(path, json, Encoding.UTF8);
        }

        public static (bool result, T asset) LoadStreaming<T>(string saveName)
        {
            var fileName = typeof(T).Name;
            return LoadStreaming<T>(saveName, fileName);
        }

        public static (bool result, T asset) LoadStreaming<T>(string saveName, string fileName)
        {
            var path = Path.Combine(StreamingAssetsPath, saveName, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден в StreamingAssets/{saveName}.");
                return (false, default);
            }

            try
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                var asset = JsonConvert.DeserializeObject<T>(json, JsonSettings);
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
