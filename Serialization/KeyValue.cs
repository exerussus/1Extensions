using System;
using System.Collections.Generic;

namespace Exerussus._1Extensions.Serialization
{
    [Serializable]
    public class KeyValue<TKey, TValue>
    {
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
    }
}