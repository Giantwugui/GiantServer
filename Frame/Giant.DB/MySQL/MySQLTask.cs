using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<T> : DBTask<T>
    {
        public string TableName { get; set; }

        public MySQLService Service
        {
            get { return this.DBService.Service as MySQLService; }
        }

        public MySqlCommand GetCommand()
        {
            return this.Service.GetCommand();
        }

        public TResult BuildData<TResult>(Dictionary<string, object> pairs) where TResult : MySqlData
        {
            return MySqlDataFactory.BuildInstance<TResult>(pairs);
        }
    }
}
