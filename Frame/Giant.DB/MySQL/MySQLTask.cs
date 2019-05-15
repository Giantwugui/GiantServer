using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<T> : DBTask<T>
    {
        public string TableName { get; set; }

        public MySQLTask()
        { 
        }

        public MySQLService Service
        {
            get { return this.DBService.Service as MySQLService; }
        }

        public MySqlCommand GetCommand()
        {
            return this.Service.GetCommand();
        }

        public TResult BuildData<TResult>(Dictionary<string, object> pairs) where TResult : class
        {
            return MySqlDataFactory.BuildInstance<TResult>(pairs);
        }
    }
}
