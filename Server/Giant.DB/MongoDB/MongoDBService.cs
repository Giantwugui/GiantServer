﻿using Giant.Core;
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
                Servers = servers,
                //ReadPreference = ReadPreference.PrimaryPreferred
            };

            mongoUrl = builder.ToMongoUrl();//mongodb://user:pass@127.0.0.1:27017/db
            client = new MongoClient(mongoUrl);
            database = client.GetDatabase(dbName);
        }

        public void Start()
        {
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            IMongoCollection<T> reCollection = null;
            if (collections.TryGetValue(collectionName, out var collection))
            {
                reCollection = collection as IMongoCollection<T>;
            }

            if (reCollection == null)
            {
                reCollection = database.GetCollection<T>(collectionName);
                collections[collectionName] = reCollection;
            }

            return reCollection;
        }
    }
}
