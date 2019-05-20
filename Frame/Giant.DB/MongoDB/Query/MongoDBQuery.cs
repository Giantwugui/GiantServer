using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBQuery<T> : MongoDBTask<T>
    {
        private readonly FilterDefinition<T> definition;

        public MongoDBQuery(string collectionName, Expression<Func<T, bool>> filter)
        {
            this.definition = filter;
            this.CollectionName = collectionName;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.GetCollection<T>(this.CollectionName);
                var result = await collection.FindSync<T>(this.definition).FirstOrDefaultAsync();

                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }

    public class MongoDBQueryBatch<T> : MongoDBTask<List<T>>
    {
        private readonly FindOptions<T> options;
        private readonly FilterDefinition<T> definition;

        public MongoDBQueryBatch(string collectionName, Expression<Func<T, bool>> filter, FindOptions<T> options = null)
        {
            this.CollectionName = collectionName;
            this.definition = filter;
            this.options = options;
        }

        /// <summary>
        /// 批量查找，并排序
        /// sort eg(new BsonElement("Level", 1),)
        /// </summary>
        /// <param name="service"></param>
        /// <param name="collectionName"></param>
        /// <param name="filter">查询条件</param>
        /// <param name="limit">查询数目</param>
        /// <param name="skip">跳过数目</param>
        /// <param name="sort">排序条件</param>
        public MongoDBQueryBatch(string collectionName, Expression<Func<T, bool>> filter, int limit, int skip, BsonDocument sort)
        {
            this.CollectionName = collectionName;
            this.definition = filter;
            this.options = new FindOptions<T>
            {
                Limit = limit,
                Skip = skip,
                Sort = sort
            };
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.GetCollection<T>(this.CollectionName);
                var cursor = collection.FindSync<T>(this.definition, this.options);

                List<T> resultList = new List<T>();
                while (await cursor.MoveNextAsync())
                {
                    resultList.AddRange(cursor.Current);
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
