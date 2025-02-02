using System;

namespace Exerussus._1Extensions.Serialization
{
    [Serializable]
    public class KeyValue<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}