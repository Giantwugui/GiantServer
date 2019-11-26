using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBUpdate<T> : MongoDBTask<bool>
    {
        private readonly T item;
        private readonly FilterDefinition<T> definition;


        public MongoDBUpdate(string collectionName, Expression<Func<T, bool>> filter, T item)
        {
            this.item = item;
            definition = filter;
            CollectionName = collectionName;
        }

        public override async Task Run()
        {
            var collection = GetCollection<T>(CollectionName);
            await collection.FindOneAndReplaceAsync(definition, item);

            SetResult(true);
        }
    }
}
