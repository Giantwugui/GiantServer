using Giant.DataTask;
using MySql.Data.MySqlClient;

namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<TResult> : DataTask<TResult>
    {
        public override IDataService DataService => this.DBService;

        public IMultDBService DBService => DataBaseService.Instance;

        public MySQLService Service
        {
            get { return DBService as MySQLService; }
        }

        public MySqlConnection GetConnection()
        {
            return this.Service.GetConnection();
        }
    }
}
