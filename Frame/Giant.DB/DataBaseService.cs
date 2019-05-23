using Giant.DataTask;

namespace Giant.DB
{
    public enum DataBaseType
    {
        MySQL,
        MongoDB
    }

    public class DataBaseService : IMultDBService
    {
        private DataBaseType dbType;

        public IDBService DBService { get; private set; }//数据库服务

        public TaskPool TaskPool { get; private set; }//数据库任务池

        public static DataBaseService Instance { get; } = new DataBaseService();//数据库服务单例

        private DataBaseService()
        {
        }

        public void Init(DataBaseType dbType, string host, string dbName, string account, string passWorld, int taskCount = 1)
        {
            this.dbType = dbType;
            this.TaskPool = new TaskPool(taskCount);

            switch (dbType)
            {
                case DataBaseType.MongoDB:
                    this.DBService = new MongoDBService(host, dbName, account, passWorld);
                    break;
                case DataBaseType.MySQL:
                    this.DBService = new MySQLService(host, dbName, account, passWorld);
                    break;
            }

            this.TaskPool.Start();
        }
    }
}
