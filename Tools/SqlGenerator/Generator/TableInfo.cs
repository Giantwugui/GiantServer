using System;
using System.Collections.Generic;

namespace SqlGenerator
{
    public class ColumInfo
    {
        public Dictionary<string, Type> colume2Type = new Dictionary<string, Type>();
        public string TableName { get; private set; }

        public ColumInfo(string tableName)
        {
            TableName = tableName;
        }

        public void AddColume(string name, Type type)
        {
            colume2Type.Add(name, type);
        }
    }

}
