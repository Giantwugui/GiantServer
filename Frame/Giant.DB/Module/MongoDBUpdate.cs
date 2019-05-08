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


        public MongoDBUpdate(DBService service, string collectionName, Expression<Func<T, bool>> filter, T item)
        {
            this.item = item;
            this.DBService = service;
            this.definition = filter;
            this.CollectionName = collectionName;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.Service.GetCollection<T>(this.CollectionName);

                await collection.FindOneAndReplaceAsync(this.definition, this.item);

                SetResult(true);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
