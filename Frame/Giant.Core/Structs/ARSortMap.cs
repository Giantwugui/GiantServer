using System.Collections.Generic;

namespace Giant.Core
{
    /// <summary>
    /// 一种保证按照删除添加的先后顺序排列的map
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class ARSortMap<K,V>
    {
        private readonly List<K> keys = new List<K>();
        private readonly Dictionary<K, V> map = new Dictionary<K, V>();

        public void Add(K key, V value)
        {
            keys.Add(key);
            map.Add(key, value);
        }

        public bool TryGetValue(K key, out V value)
        {
            return map.TryGetValue(key, out value);
        }

        public V this[K index]
        {
            get
            {
                return map[index];
            }
        }

        public void Remove(K key)
        {
            if (!map.ContainsKey(key))
            {
                return;
            }

            keys.Remove(key);
            map.Remove(key);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in keys)
            {
                yield return new KeyValuePair<K, V>(key, map[key]);
            }
        }
    }
}
