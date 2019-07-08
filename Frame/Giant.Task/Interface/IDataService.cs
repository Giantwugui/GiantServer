namespace Giant.DataTask
{
    //基础数据服务(数据库 Redis)
    public interface IDataService
    {
        TaskPool TaskPool { get; }
    }

    #region DB

    //数据库服务基础接口(mongodb，mysql)
    public interface IDBService
    {
        void Start();
    }

    //提供多种数据库服务
    public interface IMultDBService : IDataService
    {
        IDBService DBService { get; }
    }

    #endregion

    #region Redis

    public interface IRedisService : IDataService
    {
    }

    #endregion
}
