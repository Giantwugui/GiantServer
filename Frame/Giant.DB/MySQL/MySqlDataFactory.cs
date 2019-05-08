using System;
using System.Collections.Generic;
using System.Reflection;

namespace Giant.DB.MySQL
{
    public class MySqlDataFactory
    {
        public static TResult BuildInstance<TResult>(Dictionary<string, object> dictionary) where TResult : MySqlData
        {
            TResult result = Activator.CreateInstance<TResult>();

            var props = typeof(TResult).GetProperties(BindingFlags.Public);

            foreach (var prop in props)
            {
                if (dictionary.TryGetValue(prop.Name, out var value))
                {
                    prop.SetValue(result, value);
                }
            }

            return result;
        }
    }
}
