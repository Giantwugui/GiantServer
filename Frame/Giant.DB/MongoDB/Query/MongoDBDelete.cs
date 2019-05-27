using Giant.Log;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBDelete<T> : MongoDBTask<DeleteResult>
    {
        private readonly Expression<Func<T, bool>> filter;

        public MongoDBDelete(string collectionName, Expression<Func<T, bool>> filter)
        {
            this.filter = filter;
            this.CollectionName = collectionName;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.GetCollection<T>(this.CollectionName);
                var result = await collection.DeleteOneAsync<T>(this.filter);

                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
                Logger.Error(ex);
            }
        }
    }

    public class MongoDBDeleteBatch<T> : MongoDBTask<DeleteResult>
    {
        private readonly Expression<Func<T, bool>> filter;

        public MongoDBDeleteBatch(string collectionName, Expression<Func<T, bool>> filter)
        {
            this.filter = filter;
            this.CollectionName = collectionName;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.GetCollection<T>(this.CollectionName);
                var result = await collection.DeleteManyAsync<T>(this.filter);

                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
                Logger.Error(ex);
            }
        }
    }
}
