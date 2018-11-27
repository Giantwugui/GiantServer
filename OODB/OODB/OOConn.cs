using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace OODB
{
    public class OOConn : IDisposable
    {

        public OOConn(string url, int port, string dbName, string userName, string userPass)
        {

            MongoUrlBuilder urlBuilder = new MongoUrlBuilder
            {
                Server = new MongoServerAddress(url, port),
                DatabaseName = dbName,
                Username = userName,
                Password = userPass,
                W = WriteConcern.W1.W
            };

            MongoServerSettings setting = MongoServerSettings.FromUrl(urlBuilder.ToMongoUrl());

            mServer = new MongoServer(setting);
            mServer.Connect();
            Database = mServer.GetDatabase(dbName);
        }

        public OOConn(string servers, string dbName, string userName, string userPass)
        {

            string[] serverArray = servers.Split(';');
            List<MongoServerAddress> serverList = new List<MongoServerAddress>();
            for(int i = 0 ; i < serverArray.Length; ++i)
            {
                serverList.Add(new MongoServerAddress(serverArray[i]));
            }


            MongoUrlBuilder urlBuilder = new MongoUrlBuilder
            {
                Servers = serverList,
                DatabaseName = dbName,
                Username = userName,
                Password = userPass,
                ReadPreference = ReadPreference.PrimaryPreferred,
                W = WriteConcern.W1.W
            };

            MongoServerSettings setting = MongoServerSettings.FromUrl(urlBuilder.ToMongoUrl());


            mServer = new MongoServer(setting);
            mServer.Connect();
            Database = mServer.GetDatabase(dbName);
        }

        public OOConn(string servers, string dbName, string userName, string userPass, int connectMode)
        {
            string[] serverArray = servers.Split(';');
            List<MongoServerAddress> serverList = new List<MongoServerAddress>();
            for (int i = 0; i < serverArray.Length; ++i)
            {
                serverList.Add(new MongoServerAddress(serverArray[i]));
            }


            MongoUrlBuilder urlBuilder = new MongoUrlBuilder
            {
                Servers = serverList,
                DatabaseName = dbName,
                Username = userName,
                Password = userPass,
                ReadPreference = ReadPreference.PrimaryPreferred,
                ConnectionMode = (ConnectionMode)connectMode,
                W = WriteConcern.W1.W
            };

            MongoServerSettings setting = MongoServerSettings.FromUrl(urlBuilder.ToMongoUrl());

            mServer = new MongoServer(setting);
            mServer.Connect();
            Database = mServer.GetDatabase(dbName);
        }

        public void Dispose()
        {
        }

        public const int DefaultPort = 27017;


        internal readonly MongoDatabase Database;
        readonly MongoServer mServer;
    }
}
