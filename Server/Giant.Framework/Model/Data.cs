using System.Collections.Generic;

namespace Giant.Framework
{
    public class Data
    {
        public int Id { get; set; }
        public Dictionary<string, string> Params { get; set; }

        public string GetString(string key)
        {
            Params.TryGetValue(key, out string value);
            return value;
        }

        public int GetInt(string key)
        {
            int value = 0;
            if (Params.TryGetValue(key, out string strV))
            {
                int.TryParse(strV, out value);
            }
            return value;
        }

        public long GetLong(string key)
        {
            long value = 0;
            if (Params.TryGetValue(key, out string strV))
            {
                long.TryParse(strV, out value);
            }
            return value;
        }

        public float GetFloat(string key)
        {
            float value = 0;
            if (Params.TryGetValue(key, out string strV))
            {
                float.TryParse(strV, out value);
            }
            return value;
        }

        public bool GetBool(string key)
        {
            if (Params.TryGetValue(key, out string strV))
            {
                return strV == "1" || strV == "true";
            }
            return false;
        }
    }
}
