using System.Collections.Generic;
using System.Text;

namespace Giant.DB
{
    /// <summary>
    /// SQL语句构建器
    /// </summary>
    public class MSSQLBuilder
    {
        public MSSQLBuilder(string tableName, string dbName = "")
        {
            if (string.IsNullOrEmpty(dbName))
            {
                mTableName = "[" + tableName + "]";
            }
            else
            {
                mTableName = "[" + dbName + "].[dbo].[" + tableName + "]";
            }
        }

        public void SetPrimaryKey(params string[] keys)
        {
            mPrimaryKey = new List<string>(keys);
        }

        public void AddCharValue(string key, string value)
        {
            mCharValue[key] = value;
        }

        public void AddNumValue(string key, object value)
        {
            mNumValue[key] = value.ToString();
        }

        public string ToInsertSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + mTableName);
            sb.Append("(");

            bool state = false;
            foreach (KeyValuePair<string, string> kv in mCharValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append("[" + kv.Key + "]");
            }
            foreach (KeyValuePair<string, string> kv in mNumValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append("[" + kv.Key + "]");
            }

            sb.Append(")VALUES(");

            state = false;

            foreach (KeyValuePair<string, string> kv in mCharValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append("'" + kv.Value + "'");
            }
            foreach (KeyValuePair<string, string> kv in mNumValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append(kv.Value);
            }

            sb.Append(");");

            return sb.ToString();
        }
        public string ToAutoUpdateSql()
        {
            StringBuilder sb = new StringBuilder();

            bool state = false;

            sb.Append("IF EXISTS(SELECT * FROM " + mTableName + " WHERE ");

            foreach (string key in mPrimaryKey)
            {
                if (state)
                {
                    sb.Append(" AND ");
                }
                else
                {
                    state = true;
                }

                sb.Append("[" + key + "]=");

                if (mCharValue.ContainsKey(key))
                {
                    sb.Append("'" + mCharValue[key] + "'");
                }
                else
                {
                    sb.Append(mNumValue[key]);
                }
            }
            sb.Append(")\r\n");

            sb.Append("UPDATE " + mTableName + " SET ");

            state = false;
            foreach (KeyValuePair<string, string> kv in mCharValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append(kv.Key + "='" + kv.Value + "'");
            }

            foreach (KeyValuePair<string, string> kv in mNumValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }
                sb.Append(kv.Key + "=" + kv.Value);
            }

            sb.Append(" WHERE ");
            state = false;
            foreach (string key in mPrimaryKey)
            {
                if (state)
                {
                    sb.Append(" AND ");
                }
                else
                {
                    state = true;
                }

                sb.Append("[" + key + "]=");

                if (mCharValue.ContainsKey(key))
                {
                    sb.Append("'" + mCharValue[key] + "'");
                }
                else
                {
                    sb.Append(mNumValue[key]);
                }
            }

            sb.Append("; \r\n");
            sb.Append("ELSE \r\n");

            sb.Append("INSERT INTO " + mTableName);
            sb.Append("(");

            state = false;
            foreach (KeyValuePair<string, string> kv in mCharValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append("[" + kv.Key + "]");
            }
            foreach (KeyValuePair<string, string> kv in mNumValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append("[" + kv.Key + "]");
            }

            sb.Append(")VALUES(");

            state = false;

            foreach (KeyValuePair<string, string> kv in mCharValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append("'" + kv.Value + "'");
            }
            foreach (KeyValuePair<string, string> kv in mNumValue)
            {
                if (state)
                {
                    sb.Append(",");
                }
                else
                {
                    state = true;
                }

                sb.Append(kv.Value);
            }

            sb.Append(");");

            return sb.ToString();
        }

        readonly string mTableName;
        List<string> mPrimaryKey = new List<string>();
        Dictionary<string, string> mCharValue = new Dictionary<string, string>();
        Dictionary<string, string> mNumValue = new Dictionary<string, string>();
    }
}