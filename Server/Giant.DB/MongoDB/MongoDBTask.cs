using Giant.Core;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DB.MongoDB
{
    public abstract class MongoDBTask<T> : DataTask<T>
    {
        public string CollectionName { get; set; }

        public override IDataService DataService => DBService;

        public IMultiDBService DBService => DBServiceComponent.Instance;

        public MongoDBService Service
        {
            get { return DBService.DBService as MongoDBService; }
        }

        public IMongoCollection<TDocumnet> GetCollection<TDocumnet>(string collectionName)
        {
            return Service.GetCollection<TDocumnet>(collectionName);
        }

        public static async Task<List<string>> GetIndex<TDocumnet>(IMongoCollection<TDocumnet> collection)
        {
            List<string> indexList = new List<string>();
            var cursor = await collection.Indexes.ListAsync();
            while (cursor.MoveNext())
            {
                if (cursor.Current == null) continue;
                foreach (var kv in cursor.Current)
                {
                    indexList.Add(kv.GetValue("name").AsString);
                }
            }
            return indexList;
        }
    }
}
