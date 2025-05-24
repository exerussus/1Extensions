using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

        /// <summary>
        /// Извлекает элемент из словаря по указанному ключу и удаляет его из словаря.
        /// </summary>
        /// <param name="dictionary">Словарь, из которого нужно извлечь элемент.</param>
        /// <param name="key">Ключ элемента, который нужно извлечь.</param>
        /// <param name="item">Возвращаемый элемент.</param>
        /// <returns>Возвращает true, если элемент был извлечен.</returns>
        public static bool TryPop<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary, TCollection key, out TItem item)
        {
            return dictionary.Remove(key, out item);
        }  
        
        /// <summary>
        /// Устанавливает newItem по ключу, и возвращает старый, если был.
        /// </summary>
        /// <param name="dictionary">Словарь, в котором нужно установить элемент.</param>
        /// <param name="key">Ключ элемента, который нужно установить.</param>
        /// <param name="newItem">Новый элемент.</param>
        /// <param name="oldItem">Старый элемент.</param>
        /// <returns>True, если старый элемент был установлен.</returns>
        public static bool TryReplace<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary, TCollection key, TItem newItem, out TItem oldItem)
        {
            if (!dictionary.TryGetValue(key, out oldItem))
            {
                dictionary[key] = newItem;
                return false;
            }
            oldItem = dictionary[key];
            dictionary[key] = newItem;
            return true;
        }
        
        public static bool TryGetRandomKey<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary, out TCollection randomKey)
        {
            if (dictionary.Count == 0)
            {
                randomKey = default;
                return false;
            }

            randomKey = dictionary.GetRandomKey();
            return true;
        }
        
        public static TCollection GetRandomKey<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary)
        {
#if UNITY_EDITOR
            if (dictionary.Count == 0) throw new InvalidOperationException("Cannot get a random key from an empty dictionary.");
#endif

            var index = Random.Range(0, dictionary.Count);
            var enumerator = dictionary.GetEnumerator();

            while (index-- >= 0 && enumerator.MoveNext()) { }

            return enumerator.Current.Key;
        }
        
        public static (TCollection key, TItem value) GetRandomKeyValue<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary)
        {
#if UNITY_EDITOR
            if (dictionary.Count == 0) throw new InvalidOperationException("Cannot get a random key from an empty dictionary.");
#endif

            var index = Random.Range(0, dictionary.Count);
            var enumerator = dictionary.GetEnumerator();

            while (index-- >= 0 && enumerator.MoveNext()) { }

            return (enumerator.Current.Key, enumerator.Current.Value);
        }
        
        public static TItem PopRandomValue<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary)
        {
#if UNITY_EDITOR
            if (dictionary.Count == 0) throw new InvalidOperationException("Cannot pop from an empty dictionary.");
#endif
            
            var key = dictionary.GetRandomKey();
            var value = dictionary[key];
            dictionary.Remove(key);
            return value;
        }
        
        public static (TCollection key, int amount) PopAmountFromRandomKey<TCollection>(this Dictionary<TCollection, int> dictionary, int amount = 1)
        {
#if UNITY_EDITOR
            if (dictionary.Count == 0) throw new InvalidOperationException("Cannot pop from an empty dictionary.");
#endif
            
            var index = Random.Range(0, dictionary.Count);
            var enumerator = dictionary.GetEnumerator();

            while (index-- >= 0 && enumerator.MoveNext()) { }

            var value = dictionary[enumerator.Current.Key];
            var resultAmount = Mathf.Min(amount, value);
            value -= resultAmount;

            if (value < 1) dictionary.Remove(enumerator.Current.Key);
            else dictionary[enumerator.Current.Key] = value;
            
            return (enumerator.Current.Key, resultAmount);
        }
        
        public static (TCollection key, TItem value) PopRandomKeyValue<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary)
        {
#if UNITY_EDITOR
            if (dictionary.Count == 0) throw new InvalidOperationException("Cannot pop from an empty dictionary.");
#endif
            
            var key = dictionary.GetRandomKey();
            var value = dictionary[key];
            dictionary.Remove(key);
            return (key, value);
        }
        
        public static TItem GetRandomValue<TCollection, TItem>(this Dictionary<TCollection, TItem> dictionary)
        {
#if UNITY_EDITOR
            if (dictionary.Count == 0) throw new InvalidOperationException("Cannot get a random value from an empty dictionary.");
#endif

            var key = dictionary.GetRandomKey();
            return dictionary[key];
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
        /// Удаляет все строки из массива source, которые присутствуют в коллекции exclude, и возвращает новый массив.
        /// </summary>
        /// <param name="source">Исходный массив строк.</param>
        /// <param name="exclude">Коллекция строк для исключения.</param>
        /// <returns>Изменённый массив строк без исключённых элементов.</returns>
        public static string[] RemoveAll(this string[] source, IEnumerable<string> exclude)
        {
            if (source == null) return Array.Empty<string>();
            if (exclude == null) return source;

            var excludeSet = new HashSet<string>(exclude);
            return source.Where(item => !excludeSet.Contains(item)).ToArray();
        }
        
        /// <summary>
        /// Удаляет все строки из массива source, которые присутствуют в коллекции exclude, и возвращает новый массив.
        /// </summary>
        /// <param name="source">Исходный массив строк.</param>
        /// <param name="exclude">Коллекция строк для исключения.</param>
        /// <returns>Изменённый массив строк без исключённых элементов.</returns>
        public static string[] RemoveAll(this string[] source, HashSet<string> exclude)
        {
            if (source == null) return Array.Empty<string>();
            if (exclude == null) return source;

            return source.Where(item => !exclude.Contains(item)).ToArray();
        }
        
        /// <summary>
        /// Удаляет все строки из массива source, которые присутствуют в коллекции exclude, и возвращает source.
        /// </summary>
        /// <param name="source">Исходный массив строк.</param>
        /// <param name="exclude">Коллекция строк для исключения.</param>
        /// <returns>Изменённый массив строк без исключённых элементов.</returns>
        public static List<string> RemoveAll(this List<string> source, IEnumerable<string> exclude)
        {
            if (source == null) return new();
            if (exclude == null) return source;

            var excludeSet = new HashSet<string>(exclude);
            for (int i = source.Count - 1; i >= 0; i--)
            {
                var item = source[i];
                if (excludeSet.Contains(item)) source.RemoveAt(i);
            }
            return source;
        }
        
        /// <summary> Возвращает true, если коллекции listA и listB содержат одинаковые уникальные строки. </summary>
        public static bool Equal(this IEnumerable<string> listA, IEnumerable<string> listB)
        {
            var setA = new HashSet<string>(listA);
            var setB = new HashSet<string>(listB);
            return setA.SetEquals(setB);
        }
        
        /// <summary> Возвращает true, если коллекции listA и listB содержат одинаковые уникальные строки. </summary>
        public static bool Equal(this IEnumerable<string> first, IEnumerable<string> second, out HashSet<string> firstSet, out HashSet<string> secondSet)
        {
            firstSet = new HashSet<string>(first);
            secondSet = new HashSet<string>(second);
            return firstSet.SetEquals(secondSet);
        }
        
        /// <summary>
        /// Удаляет все строки из массива source, которые присутствуют в коллекции exclude, и возвращает source.
        /// </summary>
        /// <param name="source">Исходный массив строк.</param>
        /// <param name="exclude">Коллекция строк для исключения.</param>
        /// <returns>Изменённый массив строк без исключённых элементов.</returns>
        public static List<string> RemoveAll(this List<string> source, HashSet<string> exclude)
        {
            if (source == null) return new();
            if (exclude == null) return source;

            for (int i = source.Count - 1; i >= 0; i--)
            {
                var item = source[i];
                if (exclude.Contains(item)) source.RemoveAt(i);
            }
            return source;
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
        public static bool IsNotEmpty<T>(this T[] array) => array is { Length: > 0 };
        
        /// <summary>
        /// Проверяет, содержит ли массив хотя бы один элемент.
        /// </summary>
        /// <param name="collection">Коллекция для проверки.</param>
        /// <returns>true, если массив не пустой; иначе false.</returns>
        public static bool IsNotEmpty<T>(this List<T> collection) => collection is { Count: > 0 };
        
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
        /// Проверяет, содержит ли исходный список хотя бы один элемент из целевого списка.
        /// </summary>
        /// <param name="originCollection">Исходный список.</param>
        /// <param name="targetCollection">Целевой список для проверки.</param>
        /// <returns>true, если исходный список содержит хотя бы один элемент из целевого списка; иначе false.</returns>
        public static bool ContainsAny(this List<string> originCollection, string[] targetCollection)
        {
            if (targetCollection is not { Length: > 0 }) return false;
            if (originCollection is not { Count: > 0 }) return false;

            var hashSet = new HashSet<string>(originCollection);
            
            foreach (var item in targetCollection) if (hashSet.Contains(item)) return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCollection">Исходный массив.</param>
        /// <param name="targetCollection">Целевой массив для проверки.</param>
        /// <returns>true, если исходный массив содержит хотя бы один элемент из целевого массива; иначе false.</returns>
        public static bool ContainsAll(this string[] sourceCollection, string[] targetCollection)
        {
            if (sourceCollection == null) return false;
            if (targetCollection == null) return true;
            if (targetCollection.Length == 0) return true;
            if (sourceCollection.Length == 0) return false;

            var sourceSet = new HashSet<string>(sourceCollection);
            foreach (var targetItem in targetCollection) if (!sourceSet.Contains(targetItem)) return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCollection">Исходный массив.</param>
        /// <param name="targetCollection">Целевой массив для проверки.</param>
        /// <returns>true, если исходный массив содержит хотя бы один элемент из целевого массива; иначе false.</returns>
        public static bool ContainsAll(this List<string> sourceCollection, string[] targetCollection)
        {
            if (sourceCollection == null) return false;
            if (targetCollection == null) return true;
            if (targetCollection.Length == 0) return true;
            if (sourceCollection.Count == 0) return false;

            var sourceSet = new HashSet<string>(sourceCollection);
            foreach (var targetItem in targetCollection) if (!sourceSet.Contains(targetItem)) return false;
            return true;
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

        /// <summary> Копирует все значения из другой коллекции. После копирования origin имеет тот же размер элементов, что и target. </summary>
        public static HashSet<T> CopyValues<T>(this HashSet<T> origin, [NotNull, InstantHandle] IEnumerable<T> target)
        {
            origin.Clear();
            origin.UnionWith(target);
            return origin;
        }

        /// <summary> Исключает все элементы, которые присутствуют в другой коллекции. </summary>
        public static HashSet<T> ExceptValues<T>(this HashSet<T> origin, [NotNull, InstantHandle] IEnumerable<T> target)
        {
            origin.ExceptWith(target);
            return origin;
        }
        
        
        /// <summary> Перемешивает элементы списка в случайном порядке (алгоритм Фишера-Йейтса) с глобальным Random. </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            Shuffle(list, new System.Random());
        }

        /// <summary> Перемешивает элементы списка в случайном порядке (алгоритм Фишера-Йейтса) с заданным сидом. </summary>
        public static void Shuffle<T>(this List<T> list, int seed)
        {
            Shuffle(list, new System.Random(seed));
        }

        /// <summary> Перемешивает элементы списка в случайном порядке (алгоритм Фишера-Йейтса) с заданным генератором случайных чисел. </summary>
        public static void Shuffle<T>(this List<T> list, System.Random rng)
        {
            if (list == null || list.Count <= 1) return;

            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}