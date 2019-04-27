using System;
using System.Collections.Generic;

namespace Giant.Share
{
    public static class LinqEx
    {
        public static void ForEach<K, V>(this Dictionary<K, V> self, Action<KeyValuePair<K, V>> action)
        {
            foreach (var kv in self) action(kv);
        }

        public static void ForEach(this Dictionary<string, string> self, Action<KeyValuePair<string, string>> action)
        {
            foreach (var kv in self) action(kv);
        }
    }
}
