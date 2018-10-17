using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantCore
{
    public class G_JsonMap
    {
        public G_JsonMap()
        {            
        }

        public void Parse(string jsonStr)
        {
        }

        //public string Tostring()
        //{
        //}

        public void Add(string key, object value)
        {
            mParam.Add(key, value);
        }

        public T GetValut<T>(string key) where T : class
        {
            if (mParam.ContainsKey(key))
            {
                return mParam[key] as T;
            }
            else
            {
                return default(T);
            }
        }

        public object this[string key]
        {
            get
            {
                if (mParam.ContainsKey(key))
                {
                    return mParam[key];
                }
                return null;
            }
            set
            {
                mParam[key] = value;
            }
        }
        Dictionary<string, object> mParam = new Dictionary<string, object>();
    }
}
