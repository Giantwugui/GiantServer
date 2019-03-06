using MongoDB.Driver;

namespace Giant.Model
{
    sealed class DBComponentSystem : AwakeSystem<DBComponent, DBConfig>
    {
        public override void Awake(DBComponent self, DBConfig dbConfig)
        {
            self.Awake(dbConfig);
        }
    }

    class DBComponent : Component
    {
        public MongoClient MongoClient { get; private set; }

        public IMongoDatabase MongoDatabase { get; private set; }


        public void Awake(DBConfig dbConfig)
        {
            MongoClient = new MongoClient(dbConfig.ConnectString);

            MongoDatabase = MongoClient.GetDatabase(dbConfig.DBName);
        }
    }
}
