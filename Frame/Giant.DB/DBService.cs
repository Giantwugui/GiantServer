using System;
using System.Threading.Tasks;

namespace Giant.DB
{
    public enum DataBaseType
    {
        MySQL,
        MongoDB
    }

    public class DBService
    {
        private readonly DataBaseType dbType;

        public IDBService Service { get; private set; }

        public DBTaskManager TaskManager { get; private set; }//数据库操作服务

        public DBService(DataBaseType dbType, int taskCount = 1)
        {
            this.dbType = dbType;

            this.TaskManager = new DBTaskManager(taskCount);
            this.TaskManager.Start();
        }

        public void Start(string host, string dbName, string account, string passWorld)
        {
            switch (dbType)
            {
                case DataBaseType.MongoDB:
                    this.Service = new MongoDBService(host, dbName, account, passWorld);
                    break;
                case DataBaseType.MySQL:
                    this.Service = new MySQLService(host, dbName, account, passWorld);
                    break;
            }
        }
    }
}
