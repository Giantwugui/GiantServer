using System.Collections.Generic;

namespace Giant.Share
{
    public class ListMap<K, V>
    {
        public readonly Dictionary<K, List<V>> dicList = new Dictionary<K, List<V>>();

        public void Add(K key, V value)
        {
            if (!dicList.TryGetValue(key, out var valueList))
            {
                valueList = new List<V>();
                dicList.Add(key, valueList);
            }

            valueList.Add(value);
        }

        public bool TryGetValue(K key, out List<V> outList)
        {
            return dicList.TryGetValue(key, out outList);
        }

        public void Remove(K key, V value)
        {
            if (!dicList.TryGetValue(key, out var valueList))
            {
                return;
            }

            valueList.Remove(value);
            if (valueList.Count == 0)
            {
                dicList.Remove(key);
            }
        }

        public IEnumerator<KeyValuePair<K, List<V>>> GetEnumerator()
        {
            return dicList.GetEnumerator();
        }
    }
}
