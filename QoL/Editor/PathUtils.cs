using System;
using System.Collections.Generic;

namespace Exerussus._1Extensions.QoL.Editor
{
    public static class PathUtils
    {
        public static string[] GetIncrementalPaths(string folderPath)
        {
            var normalizedPath = folderPath.Replace('\\', '/');

            var result = new List<string>();
            var parts = normalizedPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var currentPath = parts[0];
            result.Add(currentPath);

            for (int i = 1; i < parts.Length; i++)
            {
                currentPath += "/" + parts[i];
                result.Add(currentPath);
            }

            return result.ToArray();
        }
        
        public static (string folderPath, string folderName)[] GetIncrementalPathsWithFolderName(string folderPath)
        {
            var normalizedPath = folderPath.Replace('\\', '/');

            var result = new List<(string folderPath, string folderName)>();
            var parts = normalizedPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var currentPath = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                result.Add((currentPath, parts[i]));
                currentPath += "/" + parts[i];
            }

            return result.ToArray();
        }
    }
}