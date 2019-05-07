using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Giant.DB.MongoDB
{
    public class MongoDBQuery<T> : MongoDBTask<T>
    {
        private FilterDefinition<T> definition;

        public MongoDBQuery(DBService service, string collectionName, Expression<Func<T, bool>> filter)
        {
            this.Service = service;
            this.CollectionName = collectionName;
            this.definition = filter;
        }

        public override async Task Run()
        {
            try
            {
                var collection = (this.Service.Service as MongoDBService).GetCollection<T>(this.CollectionName);

                var cursor = collection.FindSync<T>(this.definition);
                var result = await cursor.FirstAsync();

                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
