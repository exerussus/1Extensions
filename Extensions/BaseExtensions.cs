using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Exerussus._1Extensions.Scripts.Extensions
{
    public static class BaseExtensions
    {
        /// <summary>
        /// Извлекает элемент из словаря по указанному ключу и удаляет его из словаря.
        /// </summary>
        /// <param name="dictionary">Словарь, из которого нужно извлечь элемент.</param>
        /// <param name="key">Ключ элемента, который нужно извлечь.</param>
        /// <returns>Извлеченный элемент.</returns>
        public static TItem Pop<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary, TCollection key)
        {
            var item = dictionary[key];
            dictionary.Remove(key);
            return item;
        }
        
        public static T PopRandom<T>(this List<T> list)
        {
#if UNITY_EDITOR
            if (list.Count == 0) Debug.LogError("Пустой лист");
#endif
            if (list.Count == 1) return list.PopFirst();
            var rValue = Random.Range(0, list.Count);
            return list.Pop(rValue);
        }
        
        /// <summary>
        /// Извлекает элемент из списка по указанному индексу и удаляет его из списка.
        /// </summary>
        /// <param name="collection">Список, из которого нужно извлечь элемент.</param>
        /// <param name="index">Индекс элемента, который нужно извлечь.</param>
        /// <returns>Извлеченный элемент.</returns>
        public static T Pop<T>(this List<T> collection, int index)
        {
            var item = collection[index];
            collection.RemoveAt(index);
            return item;
        }
        
        /// <summary>
        /// Извлекает первый элемент из списка и удаляет его из списка.
        /// </summary>
        /// <param name="collection">Список, из которого нужно извлечь первый элемент.</param>
        /// <returns>Извлеченный элемент.</returns>
        public static T PopFirst<T>(this List<T> collection)
        {
            var item = collection[0];
            collection.RemoveAt(0);
            return item;
        }
        
        /// <summary>
        /// Извлекает последний элемент из списка и удаляет его из списка.
        /// </summary>
        /// <param name="collection">Список, из которого нужно извлечь последний элемент.</param>
        /// <returns>Извлеченный элемент.</returns>
        public static T PopLast<T>(this List<T> collection)
        {
            var item = collection[^1];
            collection.Remove(item);
            return item;
        }
        
        /// <summary>
        /// Получает случайный элемент из массива.
        /// </summary>
        /// <param name="collection">Массив, из которого нужно выбрать случайный элемент.</param>
        /// <returns>Случайный элемент из массива.</returns>
        public static T GetRandomItem<T>(this T[] collection)
        {
            if (collection.Length == 0) Debug.LogError("Пустой массив");
            if (collection.Length == 1) return collection[0];
            var rValue = Random.Range(0, collection.Length);
            return collection[rValue];
        }
        
        /// <summary>
        /// Получает случайный элемент из списка.
        /// </summary>
        /// <param name="collection">Список, из которого нужно выбрать случайный элемент.</param>
        /// <returns>Случайный элемент из списка.</returns>
        public static T GetRandomItem<T>(this List<T> collection)
        {
            var rValue = Random.Range(0, collection.Count);
            return collection[rValue];
        }
        
        /// <summary>
        /// Добавляет элементы из другого массива в список, избегая дублирования.
        /// </summary>
        /// <param name="list">Список, в который нужно добавить элементы.</param>
        /// <param name="other">Массив элементов, которые нужно добавить.</param>
        /// <returns>Список с добавленными уникальными элементами.</returns>
        public static List<T> AddUnique<T>(this List<T> list, T[] other)
        {
            foreach (var item in other) if (item != null && !list.Contains(item)) list.Add(item);
            return list;
        }
        
        /// <summary>
        /// Добавляет элемент в список, если его там нет.
        /// </summary>
        /// <param name="list">Список, в который нужно добавить элемент.</param>
        /// <param name="item">Элемент, который нужно добавить.</param>
        /// <returns>Список с добавленным элементом, если он не был добавлен ранее.</returns>
        public static List<T> AddUnique<T>(this List<T> list, T item)
        {
            if (item != null && !list.Contains(item)) list.Add(item);
            return list;
        }

        /// <summary>
        /// Проверяет, содержит ли массив хотя бы один элемент.
        /// </summary>
        /// <param name="array">Массив для проверки.</param>
        /// <returns>true, если массив не пустой; иначе false.</returns>
        public static bool IsNotEmpty<T>(this T[] array)
        {
            return array is { Length: > 0 };
        }
        
        /// <summary>
        /// Поворачивает вектор в плоскости XoY на заданный угол.
        /// </summary>
        /// <param name="value">Вектор до поворота.</param>
        /// <param name="angle">Угол поворота в градусах.</param>
        /// <returns>Новый вектор после поворота.</returns>
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
        /// Преобразует вектор направления в кватернион поворота в плоскости XoY.
        /// </summary>
        /// <param name="direction">Вектор направления.</param>
        /// <returns>Кватернион, представляющий поворот.</returns>
        public static Quaternion ToQuaternion(this Vector2 direction)
        {
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, angle);
        }
        
        /// <summary>
        /// Преобразует кватернион поворота в вектор направления в плоскости XoY.
        /// </summary>
        /// <param name="rotation">Кватернион поворота.</param>
        /// <returns>Вектор направления.</returns>
        public static Vector2 ToDirection(this Quaternion rotation)
        {
            float angle = rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            return direction;
        }

        /// <summary>
        /// Пытается удалить элемент из списка. Возвращает true, если элемент был удален, и false, если элемент не найден.
        /// </summary>
        /// <param name="collection">Список, из которого нужно удалить элемент.</param>
        /// <param name="item">Элемент, который нужно удалить.</param>
        /// <returns>true, если элемент был удален; иначе false.</returns>
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

        /// <summary>
        /// Проверяет, содержит ли исходный список хотя бы один элемент из целевого списка.
        /// </summary>
        /// <param name="originCollection">Исходный список.</param>
        /// <param name="targetCollection">Целевой список для проверки.</param>
        /// <returns>true, если исходный список содержит хотя бы один элемент из целевого списка; иначе false.</returns>
        public static bool ContainsAny<T>(this List<T> originCollection, List<T> targetCollection)
        {
            foreach (var item in targetCollection)
            {
                if (originCollection.Contains(item)) return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет, содержит ли исходный массив хотя бы один элемент из целевого массива.
        /// </summary>
        /// <param name="originCollection">Исходный массив.</param>
        /// <param name="targetCollection">Целевой массив для проверки.</param>
        /// <returns>true, если исходный массив содержит хотя бы один элемент из целевого массива; иначе false.</returns>
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
        
        /// <summary>
        /// Добавляет элемент в массив, создавая новый массив с добавленным элементом.
        /// </summary>
        /// <param name="collection">Исходный массив.</param>
        /// <param name="item">Элемент, который нужно добавить.</param>
        /// <returns>Новый массив с добавленным элементом.</returns>
        public static T[] Add<T>(this T[] collection, T item)
        {
            if (collection == null) collection = new [] { item };
            
            else
            {
                var newCollection = new T[collection.Length + 1];
                for (int i = 0; i < collection.Length; i++) newCollection[i] = collection[i];
                newCollection[collection.Length] = item;
                collection = newCollection;
            }

            return collection;
        }
        
        /// <summary>
        /// Удаляет все вхождения указанного элемента из массива и возвращает новый массив без этого элемента.
        /// </summary>
        /// <param name="collection">Исходный массив.</param>
        /// <param name="item">Элемент, который нужно удалить.</param>
        /// <returns>Новый массив без указанного элемента.</returns>
        public static T[] Remove<T>(this T[] collection, T item)
        {
            if (collection == null) return null;
    
            int count = 0;
            foreach (var element in collection)
            {
                if (!element.Equals(item)) count++;
            }
    
            if (count == collection.Length) return collection;
    
            T[] newCollection = new T[count];
            
            int index = 0;
            foreach (var element in collection)
            {
                if (!element.Equals(item)) newCollection[index++] = element;
            }
    
            return newCollection;
        }
        
        /// <summary>
        /// Удаляет элемент по указанному индексу из массива и возвращает новый массив без этого элемента.
        /// </summary>
        /// <param name="collection">Исходный массив.</param>
        /// <param name="index">Индекс элемента, который нужно удалить.</param>
        /// <returns>Новый массив без элемента на указанном индексе.</returns>
        public static T[] RemoveAt<T>(this T[] collection, int index)
        {
            if (collection == null || index < 0 || index >= collection.Length) return new T[] { };
            
            T[] newCollection = new T[collection.Length - 1];

            for (int i = 0; i < index; i++) newCollection[i] = collection[i];
            
            for (int i = index + 1; i < collection.Length; i++) newCollection[i - 1] = collection[i];

            return newCollection;
        }
        
        /// <summary>
        /// Удаляет все null значения из массива и возвращает новый массив без null значений.
        /// </summary>
        /// <param name="collection">Исходный массив.</param>
        /// <returns>Новый массив без null значений.</returns>
        public static T[] RemoveNulls<T>(this T[] collection) where T : class
        {
            if (collection == null) return new T[] {};

            int nonNullCount = 0;
            foreach (var item in collection)
            {
                if (item != null) nonNullCount++;
            }

            if (nonNullCount == 0) return Array.Empty<T>();
            
            T[] newCollection = new T[nonNullCount];
            int index = 0;
            foreach (var item in collection)
            {
                if (item != null) newCollection[index++] = item;
            }

            return newCollection;
        }
        
        /// <summary>
        /// Проверяет, есть ли у enum указанный битовый флаг
        /// </summary>
        public static bool HasFlag<T>(this T enumValue, T value) where T : Enum
        {        
            var enumByte = Convert.ToByte(enumValue);
            var valueByte = Convert.ToByte(value);
            return (enumByte & valueByte) == valueByte;
        }
        
        /// <summary>
        /// Проверяет, есть ли в enum хотя бы один указанный битовый флаг
        /// </summary>
        public static bool HasAnyFlag<T>(this T enumValue, T[] values) where T : Enum
        {        
            var enumByte = Convert.ToByte(enumValue);
            foreach (var value in values)
            {
                var valueByte = Convert.ToByte(value);
                if ((enumByte & valueByte) == valueByte) return true;
            }

            return false;
        }
        
        /// <summary>
        /// Проверяет, есть ли в enum хотя бы один указанный битовый флаг
        /// </summary>
        public static bool HasAnyFlag<T>(this T enumValue, List<T> values) where T : Enum
        {        
            var enumByte = Convert.ToByte(enumValue);
            foreach (var value in values)
            {
                var valueByte = Convert.ToByte(value);
                if ((enumByte & valueByte) == valueByte) return true;
            }

            return false;
        }
    }
}