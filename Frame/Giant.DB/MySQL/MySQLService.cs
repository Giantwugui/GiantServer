using MySql.Data.MySqlClient;
using System.Data;

namespace Giant.DB
{
    public class MySQLService : IDBService
    {
        private readonly MySqlConnection connection;

        public MySQLService(string host, string dbName, string account, string passWorld)
        {
            string connStr = $"Server={host};Database={dbName};Uid={account};Pwd = {passWorld}; ";
            this.connection = new MySqlConnection(connStr);

            this.connection.Open();
        }

        public void Start()
        {
        }

        public MySqlCommand GetCommand()
        {
            if (this.connection.State == ConnectionState.Open)
            {
                return this.connection.CreateCommand();
            }
            return null;
        }
    }
}
