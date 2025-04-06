using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exerussus._1Extensions.Serialization
{
    [Serializable]
    public class KeyValue<TKey, TValue>
    {
        public KeyValue() { }

        public KeyValue(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
        
        public void Deconstruct(out TKey key, out TValue value)
        {
            key = this.key;
            value = this.value;
        }
        
        public TKey key;
        public TValue value;
    }

    public static class KeyValueExtensions
    {
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, KeyValue<TKey, TValue> keyValue)
        {
            dictionary.Add(keyValue.key, keyValue.value);
        }
        
        public static void AddSafe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, KeyValue<TKey, TValue> keyValue)
        {
            dictionary[keyValue.key] =  keyValue.value;
        }
        
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this List<KeyValue<TKey, TValue>> dictionary)
        {
            var result = new Dictionary<TKey, TValue>(dictionary.Count);
            foreach (var keyValue in dictionary)
            {
                if (!result.TryAdd(keyValue.key, keyValue.value)) Debug.LogWarning($"Key {keyValue.key} already exists in the dictionary.");
            }
            return result;
        }
    }
}