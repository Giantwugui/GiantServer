using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBInsert<T> : MongoDBTask<bool>
    {
        private T item;

        public MongoDBInsert(string collectionName, T item)
        {
            CollectionName = collectionName;
            this.item = item;
        }

        public override async Task Run()
        {
            var collection = GetCollection<T>(CollectionName);

            await collection.InsertOneAsync(item);

            SetResult(true);
        }
    }

    public class MongoDBInsertBatch<T> : MongoDBTask<bool>
    {
        private List<T> items;

        public MongoDBInsertBatch(string collectionName, List<T> items)
        {
            CollectionName = collectionName;
            this.items = items;
        }

        public override async Task Run()
        {
            var collection = GetCollection<T>(CollectionName);

            await collection.InsertManyAsync(items);

            SetResult(true);
        }
    }
}
