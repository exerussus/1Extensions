
using UnityEngine;

namespace Exerussus._1Extensions.Prefabs
{
    public static class PrefabUtilityExtensions
    {
        public static void RevertToPrefab(this GameObject gameObject)
        {
#if UNITY_EDITOR

            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                GameObject prefabRoot = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
                if (prefabRoot != null)
                {
                    UnityEditor.PrefabUtility.RevertPrefabInstance(prefabRoot, UnityEditor.InteractionMode.UserAction);
                    Debug.Log($"Сброшены изменения для объекта {gameObject.name} до состояния префаба.");
                }
                else
                {
                    Debug.LogWarning($"Не удалось найти корневой объект префаба для {gameObject.name}.");
                }
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} не является инстансом префаба.");
            }
#endif
        }
    }
}