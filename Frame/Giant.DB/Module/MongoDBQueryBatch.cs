using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Giant.DB.MongoDB
{
    public class MongoDBQueryBatch<T> : MongoDBTask<List<T>>
    {
        private FilterDefinition<T> definition;

        public MongoDBQueryBatch(DBService service, string collectionName, Expression<Func<T, bool>> filter)
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

                List<T> resultList = new List<T>();
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<T> tempEnum = cursor.Current;
                    foreach (var curr in tempEnum)
                    {
                        resultList.Add(curr);
                    }
                }

                SetResult(resultList);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
