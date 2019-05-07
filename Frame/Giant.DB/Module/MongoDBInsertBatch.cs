using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public class MongoDBInsertBatch<T> : MongoDBTask<bool>
    {
        private List<T> items;

        public MongoDBInsertBatch(DBService service, List<T> items)
        {
            this.CollectionName = "Player";
            this.Service = service;
            this.items = items;
        }

        public override async Task Run()
        {
            try
            {
                var collection = (this.Service.Service as MongoDBService).GetCollection<T>(this.CollectionName);

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
