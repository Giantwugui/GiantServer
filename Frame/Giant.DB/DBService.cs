namespace Giant.DB
{
    public enum DataBaseType
    {
        MySQL,
        MongoDB
    }

    public class DBService
    {
        private DataBaseType dbType;

        public IService Service { get; private set; }//数据库服务

        public DBTaskPool TaskPool { get; private set; }//数据库任务池

        public static DBService Instance { get; } = new DBService();//数据库服务单例

        private DBService()
        {
        }

        public void Init(DataBaseType dbType, string host, string dbName, string account, string passWorld, int taskCount = 1)
        {
            this.dbType = dbType;
            this.TaskPool = new DBTaskPool(taskCount);

            switch (dbType)
            {
                case DataBaseType.MongoDB:
                    this.Service = new MongoDBService(host, dbName, account, passWorld);
                    break;
                case DataBaseType.MySQL:
                    this.Service = new MySQLService(host, dbName, account, passWorld);
                    break;
            }

            this.TaskPool.Start();
        }
    }
}
