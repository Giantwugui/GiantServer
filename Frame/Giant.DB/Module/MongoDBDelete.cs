using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBDelete<T> : MongoDBTask<DeleteResult>
    {
        private readonly Expression<Func<T, bool>> filter;

        public MongoDBDelete(DBService service, string collectionName, Expression<Func<T, bool>> filter)
        {
            this.DBService = service;
            this.filter = filter;
            this.CollectionName = collectionName;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.Service.GetCollection<T>(this.CollectionName);
                var result = await collection.DeleteOneAsync<T>(this.filter);

                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }

    public class MongoDBDeleteBatch<T> : MongoDBTask<DeleteResult>
    {
        private readonly Expression<Func<T, bool>> filter;

        public MongoDBDeleteBatch(DBService service, string collectionName, Expression<Func<T, bool>> filter)
        {
            this.DBService = service;
            this.filter = filter;
            this.CollectionName = collectionName;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.Service.GetCollection<T>(this.CollectionName);
                var result = await collection.DeleteManyAsync<T>(this.filter);

                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
