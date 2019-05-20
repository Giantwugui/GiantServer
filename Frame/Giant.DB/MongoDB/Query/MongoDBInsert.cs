using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBInsert<T> : MongoDBTask<bool>
    {
        private T item;

        public MongoDBInsert(string collectionName, T item)
        {
            this.CollectionName = collectionName;
            this.item = item;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.GetCollection<T>(this.CollectionName);

                await collection.InsertOneAsync(this.item);

                SetResult(true);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }

    public class MongoDBInsertBatch<T> : MongoDBTask<bool>
    {
        private List<T> items;

        public MongoDBInsertBatch(string collectionName, List<T> items)
        {
            this.CollectionName = collectionName;
            this.items = items;
        }

        public override async Task Run()
        {
            try
            {
                var collection = this.GetCollection<T>(this.CollectionName);

                await collection.InsertManyAsync(this.items);

                SetResult(true);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }
    }
}
