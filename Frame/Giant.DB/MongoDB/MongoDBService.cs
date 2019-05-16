using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Giant.DB
{
    public class MongoDBService : IDBService
    {
        private readonly MongoClient client;
        private readonly IMongoDatabase database;
        private readonly MongoUrl mongoUrl;
        private readonly Dictionary<string, object> collections = new Dictionary<string, object>();

        public MongoDBService(string host, string dbName, string account, string passWorld)
        {
            string[] hosts = host.Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<MongoServerAddress> servers = new List<MongoServerAddress>();
            foreach (var address in hosts)
            {
                string[] strs = address.Split(':');
                servers.Add(new MongoServerAddress(strs[0], int.Parse(strs[1])));
            }

            MongoUrlBuilder builder = new MongoUrlBuilder
            {
                Username = account,
                Password = passWorld,
                DatabaseName = dbName,
                Servers = servers
            };

            this.mongoUrl = builder.ToMongoUrl();//mongodb://user:pass@127.0.0.1:27017/db
            this.client = new MongoClient(mongoUrl);
            this.database = this.client.GetDatabase(dbName);
        }

        public void Start()
        {
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            IMongoCollection<T> reCollection;
            if (this.collections.TryGetValue(collectionName, out var collection))
            {
                reCollection = collection as IMongoCollection<T>;
            }
            else
            {
                reCollection = this.database.GetCollection<T>(collectionName);
                this.collections[collectionName] = reCollection;
            }

            return reCollection;
        }
    }
}
