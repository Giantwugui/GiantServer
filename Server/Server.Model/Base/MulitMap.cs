using System.Collections.Generic;

namespace Giant.Model
{
    public class MulitMap<K, V>
    {
        private Dictionary<K, List<V>> dictionary = new Dictionary<K, List<V>>();

        public void Add(K k, V v)
        {
            if (!dictionary.ContainsKey(k))
            {
                dictionary.Add(k, new List<V>());
            }

            dictionary[k].Add(v);
        }

        public List<V> Get(K k)
        {
            if (dictionary.ContainsKey(k))
            {
                return dictionary[k];
            }

            return new List<V>();
        }

        public List<V> this[K k]
        {
            get
            {
                if (!dictionary.TryGetValue(k, out List<V> list))
                {
                    list = new List<V>();
                }

                return list;
            }
        }

        public bool ContainsKey(K k)
        {
            return dictionary.ContainsKey(k);
        }

        public void Clear()
        {
            dictionary.Clear();
        }
    }
}
