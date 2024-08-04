using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class BaseExtensions
    {
        public static TI Pop<TK, TI>(this Dictionary<TK, TI> dictionary, TK key)
        {
            var item = dictionary[key];
            dictionary.Remove(key);
            return item;
        }
        
        public static T Pop<T>(this List<T> collection, int index)
        {
            var item = collection[index];
            collection.RemoveAt(index);
            return item;
        }
        
        public static T PopFirst<T>(this List<T> collection)
        {
            var item = collection[0];
            collection.RemoveAt(0);
            return item;
        }
        
        public static T PopLast<T>(this List<T> collection)
        {
            var item = collection[^1];
            collection.Remove(item);
            return item;
        }
        
        public static T GetRandomItem<T>(this T[] collection)
        {
            if (collection.Length == 0) Debug.LogError("Empty array");
            if (collection.Length == 1) return collection[0];
            var rValue = Random.Range(0, collection.Length);
            return collection[rValue];
        }
        
        public static T GetRandomItem<T>(this List<T> collection)
        {
            var rValue = Random.Range(0, collection.Count);
            return collection[rValue];
        }
        
        public static List<T> AddUnique<T>(this List<T> list, T[] other)
        {
            foreach (var item in other) if (item != null && !list.Contains(item)) list.Add(item);
            return list;
        }
        
        public static List<T> AddUnique<T>(this List<T> list, T item)
        {
            if (item != null && !list.Contains(item)) list.Add(item);
            return list;
        }

        public static bool IsNotEmpty<T>(this T[] array)
        {
            return array is { Length: > 0 };
        }
        
        /// <summary>
        /// Rotates vector left on angle in XoY plane
        /// </summary>
        /// <param name="value">vector before rotation</param>
        /// <param name="angle">angle to rotate</param>
        /// <returns></returns>
        public static Vector2 RotateVectorOnAngle(this Vector2 value, float angle)
        {
            float sinA = Mathf.Sin(angle / Mathf.Rad2Deg);
            float cosA = Mathf.Cos(angle / Mathf.Rad2Deg);
            return new Vector2(
                value.x * cosA - value.y * sinA,
                value.x * sinA + value.y * cosA
            );
        }
        
        /// <summary>
        /// Converts a Vector2 direction to a Quaternion rotation in the XoY plane.
        /// </summary>
        /// <param name="direction">Vector2 direction</param>
        /// <returns>Quaternion rotation</returns>
        public static Quaternion ToQuaternion(this Vector2 direction)
        {
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, angle);
        }
        
        public static Vector2 ToDirection(this Quaternion rotation)
        {
            float angle = rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            return direction;
        }

        public static bool TryRemove<T>(this List<T> collection, T item)
        {
            if (collection.Contains(item))
            {
                collection.Remove(item);
                return true;
            }

            Debug.Log(false);
            return false;
        }

        public static bool ContainsAny<T>(this List<T> originCollection, List<T> targetCollection)
        {
            foreach (var item in targetCollection)
            {
                if (originCollection.Contains(item)) return true;
            }

            return false;
        }

        public static bool ContainsAny(this string[] originCollection, string[] targetCollection)
        {
            if (originCollection == null || targetCollection == null) return false;
            if (originCollection.Length == 0 || targetCollection.Length == 0) return false;

            var originSet = new HashSet<string>(originCollection);
            foreach (var targetItem in targetCollection)
            {
                if (originSet.Contains(targetItem))
                {
                    return true;
                }
            }
            return false;
        }
    }
}