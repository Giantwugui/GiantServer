using Giant.DataTask;
using MongoDB.Driver;

namespace Giant.DB.MongoDB
{
    public abstract class MongoDBTask<T> : DataTask<T>
    {
        public string CollectionName { get; set; }

        public override IDataService DataService => this.DBService;

        public IMultDBService DBService => DataBaseService.Instance;

        public MongoDBService Service
        {
            get { return DBService.DBService as MongoDBService; }
        }

        public IMongoCollection<TDocumnet> GetCollection<TDocumnet>(string collectionName)
        {
            return this.Service.GetCollection<TDocumnet>(collectionName);
        }

    }
}
