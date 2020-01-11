using System;
using System.Collections.Generic;

namespace SqlGenerator
{
    public class ColumInfo
    {
        private Dictionary<string, Type> colume2Type = new Dictionary<string, Type>();
        public Dictionary<string, Type> Colume2Type => colume2Type;

        private List<string> primaryKey = new List<string>();
        public List<string> PrimaryKey => primaryKey;

        public string TableName { get; private set; }

        public ColumInfo(string tableName)
        {
            TableName = tableName;
        }

        public void AddColume(string name, Type type)
        {
            Colume2Type.Add(name, type);
        }

        public void AddPrimaryKey(string key)
        {
            primaryKey.Add(key);
        }

        public bool IsPrimaryKey(string name)
        {
            return primaryKey.Contains(name);
        }
    }

}
