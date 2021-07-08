using Giant.Core;
using MySql.Data.MySqlClient;
using System;

namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<TResult> : DataTask<TResult>
    {
        private MySqlConnection connection;

        public override IDataService DataService => DBService;

        public IMultiDBService DBService => DBServiceComponent.Instance;

        public MySQLService Service
        {
            get { return DBService.DBService as MySQLService; }
        }

        public MySqlConnection GetConnection()
        {
            connection = Service.GetConnection();
            return connection;
        }

        public override void SetException(Exception ex)
        {
            connection?.Dispose();
            base.SetException(ex);
        }
    }
}
