using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<TResult> : DBTask<TResult>
    {
        public MySQLService Service
        {
            get { return this.DBService.Service as MySQLService; }
        }

        public abstract Task Run(MySqlCommand command); 


        public MySqlCommand GetCommand()
        {
            return this.Service.GetCommand();
        }

        public TValue BuildData<TValue>(Dictionary<string, object> pairs) where TValue : class
        {
            return MySqlHelper.BuildInstance<TValue>(pairs);
        }

        //public string BuildInsertCommand<TValue>(TValue value) where TValue : class
        //{
        //    return MySqlHelper.BuldInsertCommand(value, this.TableName);
        //}
    }
}
