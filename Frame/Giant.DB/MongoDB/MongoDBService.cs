using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Giant.DB
{
    public class MongoDBService : IDBService
    {
        private readonly MongoClient client;
        private readonly IMongoDatabase database;
        private readonly MongoClientSettings clientSettings;
        private readonly Dictionary<string, object> collections = new Dictionary<string, object>();

        public MongoDBService(string host, string dbName, string account, string passWorld)
        {
            string[] hosts = host.Split(',', StringSplitOptions.RemoveEmptyEntries);
            clientSettings = new MongoClientSettings();

            List<MongoServerAddress> servers = new List<MongoServerAddress>();
            foreach (var address in hosts)
            {
                string[] strs = address.Split(':');
                servers.Add(new MongoServerAddress(strs[0], int.Parse(strs[1])));
            }
            clientSettings.Servers = servers;

            this.client = new MongoClient(clientSettings);
            this.database = this.client.GetDatabase(dbName);
        }

        public void Start(int taskCount)
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
