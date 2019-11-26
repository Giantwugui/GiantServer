using Giant.Core;
using MongoDB.Driver;

namespace Giant.DB.MongoDB
{
    public abstract class MongoDBTask<T> : DataTask<T>
    {
        public string CollectionName { get; set; }

        public override IDataService DataService => DBService;

        public IMultDBService DBService => DBServiceComponent.Instance;

        public MongoDBService Service
        {
            get { return DBService.DBService as MongoDBService; }
        }

        public IMongoCollection<TDocumnet> GetCollection<TDocumnet>(string collectionName)
        {
            return Service.GetCollection<TDocumnet>(collectionName);
        }

    }
}
