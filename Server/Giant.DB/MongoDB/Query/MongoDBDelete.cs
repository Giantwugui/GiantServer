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
            CollectionName = collectionName;
        }

        public override async Task Run()
        {
            var collection = GetCollection<T>(CollectionName);
            var result = await collection.DeleteOneAsync<T>(filter);

            SetResult(result);
        }
    }

    public class MongoDBDeleteBatch<T> : MongoDBTask<DeleteResult>
    {
        private readonly Expression<Func<T, bool>> filter;

        public MongoDBDeleteBatch(string collectionName, Expression<Func<T, bool>> filter)
        {
            this.filter = filter;
            CollectionName = collectionName;
        }

        public override async Task Run()
        {
            var collection = GetCollection<T>(CollectionName);
            var result = await collection.DeleteManyAsync<T>(filter);

            SetResult(result);
        }
    }
}
