using Giant.DataTask;
using StackExchange.Redis;

namespace Giant.Redis
{
    public abstract class RedisTask<TResult> : DataTask<TResult>
    {
        public override IDataService DataService => RedisService.Instance;

        public IDatabase Database => RedisService.Instance.Database;
    }
}