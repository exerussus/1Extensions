#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Exerussus._1Extensions.ExtensionEditor.Editor
{
    public static class NuGetForUnityAutoInstaller
    {
        private const string PackageName = "com.github-glitchenzo.nugetforunity";
        private const string PackageUrl = "https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity";
        private const string ManifestPath = "Packages/manifest.json";

        static NuGetForUnityAutoInstaller()
        {
            InstallNuGetForUnity();
        }

        public static void InstallNuGetForUnity()
        {
            string manifestFullPath = Path.Combine(Application.dataPath, "../", ManifestPath);

            if (!File.Exists(manifestFullPath))
            {
                Debug.LogError($"Файл manifest.json не найден по пути: {manifestFullPath}");
                return;
            }

            string jsonText = File.ReadAllText(manifestFullPath);

            // Проверяем, есть ли уже этот пакет
            if (jsonText.Contains($"\"{PackageName}\""))
            {
                Debug.Log($"Пакет {PackageName} уже установлен.");
                EditorPrefs.SetBool("MyPackage.NuGetInstalled", true);
                return;
            }

            // Ищем место, где добавляются зависимости
            int dependenciesIndex = jsonText.IndexOf("\"dependencies\": {");
            if (dependenciesIndex == -1)
            {
                Debug.LogError("Не найдена секция \"dependencies\" в manifest.json.");
                return;
            }

            // Ищем место для добавления новой зависимости
            int insertIndex = jsonText.IndexOf('}', dependenciesIndex); // Найти конец блока
            string newDependency = $"\"{PackageName}\": \"{PackageUrl}\"";

            // Добавляем новую зависимость
            jsonText = jsonText.Insert(insertIndex, (insertIndex > dependenciesIndex + 15 ? ",\n    " : "\n    ") + newDependency);

            // Сохраняем изменения
            File.WriteAllText(manifestFullPath, jsonText);

            Debug.Log($"Добавлен {PackageName} в manifest.json. Перезагружаем UPM...");

            // Перезагружаем зависимости UPM
            EditorPrefs.SetBool("MyPackage.NuGetInstalled", true);
            AssetDatabase.Refresh();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }
    }
}

#endif