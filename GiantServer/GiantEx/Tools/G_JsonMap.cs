using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace GiantCore
{
    public class G_JsonMap
    {
        public G_JsonMap()
        {            
        }

        public void Parse(string jsonStr)
        {
            mParam = JsonConvert.DeserializeObject(jsonStr) as Dictionary<string, object>;
        }

        public void Add(string key, object value)
        {
            mParam.Add(key, value);
        }

        public string StrValue(string key)
        {
            return mParam[key] as string;
        }

        public string Tostring()
        {
            return JsonConvert.SerializeObject(mParam);
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
