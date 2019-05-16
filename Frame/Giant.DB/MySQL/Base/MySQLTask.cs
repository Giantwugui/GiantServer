using MySql.Data.MySqlClient;

namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<TResult> : DBTask<TResult>
    {
        public MySQLService Service
        {
            get { return this.DBService.Service as MySQLService; }
        }

        public MySqlConnection GetConnection()
        {
            return this.Service.GetConnection();
        }
    }
}
