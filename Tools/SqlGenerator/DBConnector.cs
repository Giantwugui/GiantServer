using MySql.Data.MySqlClient;

namespace SqlGenerator
{
    public class DBConnector
    {
        private static readonly string connStr;

        static DBConnector()
        {
            connStr = $"data source={DBConfig.DBHost};Port=3306;Database={DBConfig.DBName};Uid={DBConfig.Account};Pwd = {DBConfig.Pwd}; ";
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }
    }
}
