namespace Giant.Core
{
    //基础数据服务(数据库 Redis)
    public interface IDataService
    {
        TaskPool TaskPool { get; }
    }
}
