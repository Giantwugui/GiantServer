using System;
using Giant.Core;
using MySql.Data.MySqlClient;

namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<TResult> : DataTask<TResult>
    {
        private MySqlConnection connection;

        public override IDataService DataService => this.DBService;

        public IMultDBService DBService => DataBaseService.Instance;

        public MySQLService Service
        {
            get { return DBService.DBService as MySQLService; }
        }

        public MySqlConnection GetConnection()
        {
            this.connection = this.Service.GetConnection();
            return connection;
        }

        public override void SetException(Exception ex)
        {
            this.connection?.Dispose();
            base.SetException(ex);
        }
    }
}
