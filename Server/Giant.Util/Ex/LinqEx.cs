using System;
using System.Collections.Generic;

namespace Giant.Util
{
    public static class LinqEx
    {
        public static void ForEach<K, V>(this Dictionary<K, V> self, Action<KeyValuePair<K, V>> action)
        {
            foreach (var kv in self) action(kv);
        }
    }
}
