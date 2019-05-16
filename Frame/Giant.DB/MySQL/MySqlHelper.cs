using System;
using System.Collections.Generic;

namespace Giant.DB.MySQL
{
    public class MySqlHelper
    {
        public static TResult BuildInstance<TResult>(Dictionary<string, object> dictionary) where TResult : class
        {
            TResult result = Activator.CreateInstance<TResult>();

            Type type = typeof(TResult);

            var props = type.GetProperties();
            var fields = type.GetFields();

            foreach (var prop in props)
            {
                if (dictionary.TryGetValue(prop.Name, out var value))
                {
                    prop.SetValue(result, value);
                }
            }

            foreach (var field in fields)
            {
                if (dictionary.TryGetValue(field.Name, out var value))
                {
                    field.SetValue(result, value);
                }
            }

            return result;
        }
    }
}
