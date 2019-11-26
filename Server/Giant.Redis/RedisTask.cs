using Giant.Core;
using StackExchange.Redis;

namespace Giant.Redis
{
    public abstract class RedisTask<TResult> : DataTask<TResult>
    {
        public override IDataService DataService => RedisComponent.Instance;

        public IDatabase Database => RedisComponent.Instance.Database;
    }
}