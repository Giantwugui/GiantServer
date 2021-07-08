using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;

namespace Giant.DB
{
    public class DBServiceComponent : InitSystem<DBType, DBConfig>, IMultiDBService
    {
        private DBType dbType;

        public IDBService DBService { get; private set; }//数据库服务

        public TaskPool TaskPool { get; private set; }//数据库任务池

        private static DBServiceComponent instance;
        public static DBServiceComponent Instance => instance;

        public DBServiceComponent() { }

        public override void Init(DBType dbType, DBConfig config)
        {
            instance = this;
            this.dbType = dbType;
            TaskPool = new TaskPool(config.TaskCount);
            TaskPool.Start();

            switch (dbType)
            {
                case DBType.MongoDB:
                    DBService = new MongoDBService(config.DBHost, config.DBName, config.Account, config.Pwd);
                    break;
                case DBType.MySQL:
                    DBService = new MySQLService(config.DBHost, config.DBName, config.Account, config.Pwd);
                    break;
            }
        }
    }
}
