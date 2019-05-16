using MongoDB.Driver;

namespace Giant.DB.MongoDB
{
    public abstract class MongoDBTask<T> : DBTask<T>
    {
        public string CollectionName { get; set; }

        public MongoDBService Service
        {
            get { return this.DBService.Service as MongoDBService; }
        }

        public IMongoCollection<TDocumnet> GetCollection<TDocumnet>(string collectionName)
        {
            return this.Service.GetCollection<TDocumnet>(collectionName);
        }

    }
}
