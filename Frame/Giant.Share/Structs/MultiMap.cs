using System.Collections.Generic;

namespace Giant.Net
{
    public class MultiMap<K,V>
    {
        private readonly Dictionary<K, V> keyValueMap = new Dictionary<K, V>();
        private readonly Dictionary<V, K> valueKeyMap = new Dictionary<V, K>();

        public bool TryGetValue(K key, out V value)
        {
            return keyValueMap.TryGetValue(key, out value);
        }

        public bool TryGetKey(V value, out K key)
        {
            return valueKeyMap.TryGetValue(value, out key);
        }

        public void AddRange(Dictionary<K,V> keyValues)
        {
            foreach (var kv in keyValues)
            {
                keyValueMap[kv.Key] = kv.Value;
                valueKeyMap[kv.Value] = kv.Key;
            }
        }
    }
}
