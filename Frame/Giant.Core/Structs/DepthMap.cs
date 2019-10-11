using System.Collections.Generic;

namespace Giant.Core
{
    public class DepthMap<K, SK, V>
    {
        public readonly Dictionary<K, Dictionary<SK, V>> depthMap = new Dictionary<K, Dictionary<SK, V>>();

        public void Add(K key, SK secondKey, V value)
        {
            if (!depthMap.TryGetValue(key, out var valueDic))
            {
                valueDic = new Dictionary<SK, V>();
                depthMap.Add(key, valueDic);
            }

            valueDic.Add(secondKey, value);
        }

        public void Remove(K key, SK secondKey)
        {
            if (!depthMap.TryGetValue(key, out var valueList))
            {
                return;
            }

            valueList.Remove(secondKey);
            if (valueList.Count == 0)
            {
                depthMap.Remove(key);
            }
        }

        public void Remove(SK secondKey)
        {
            foreach (var kv in depthMap)
            {
                if (kv.Value.ContainsKey(secondKey))
                {
                    kv.Value.Remove(secondKey);
                    break;
                }
            }
        }

        public bool TryGetValue(K key, out Dictionary<SK, V> outList)
        {
            return depthMap.TryGetValue(key, out outList);
        }

        public bool TryGetValue(K key, SK secondKey, out V value)
        {
            if (depthMap.TryGetValue(key, out var secondMap))
            {
                return secondMap.TryGetValue(secondKey, out value);
            }
            value = default;
            return false;
        }

        public bool TryGetValue(SK secondKey, out V value)
        {
            foreach (var kv in depthMap)
            {
                if (kv.Value.ContainsKey(secondKey))
                {
                    if (kv.Value.TryGetValue(secondKey, out value))
                    {
                        return true;
                    }
                }
            }
            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<K, Dictionary<SK, V>>> GetEnumerator()
        {
            return depthMap.GetEnumerator();
        }
    }
}
