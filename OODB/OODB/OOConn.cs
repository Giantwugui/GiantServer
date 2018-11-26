using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace OODB
{
    public class OOConn : IDisposable
    {

        public const int DefaultPort = 27017;


        public OOConn(string url, int port, string dbName, string userName, string userPass)
        {

            MongoUrlBuilder urlBuilder = new MongoUrlBuilder();
            urlBuilder.Server = new MongoServerAddress(url, port);
            urlBuilder.DatabaseName = dbName;
            urlBuilder.Username = userName;
            urlBuilder.Password = userPass;
            urlBuilder.W = WriteConcern.W1.W;

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


            MongoUrlBuilder urlBuilder = new MongoUrlBuilder();
            urlBuilder.Servers = serverList;
            urlBuilder.DatabaseName = dbName;
            urlBuilder.Username = userName;
            urlBuilder.Password = userPass;
            urlBuilder.ReadPreference = ReadPreference.PrimaryPreferred;
            urlBuilder.W = WriteConcern.W1.W;

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


            MongoUrlBuilder urlBuilder = new MongoUrlBuilder();
            urlBuilder.Servers = serverList;
            urlBuilder.DatabaseName = dbName;
            urlBuilder.Username = userName;
            urlBuilder.Password = userPass;
            urlBuilder.ReadPreference = ReadPreference.PrimaryPreferred;
            urlBuilder.ConnectionMode = (ConnectionMode)connectMode;
            urlBuilder.W = WriteConcern.W1.W;

            MongoServerSettings setting = MongoServerSettings.FromUrl(urlBuilder.ToMongoUrl());

            mServer = new MongoServer(setting);
            mServer.Connect();
            Database = mServer.GetDatabase(dbName);
        }

        public void Dispose()
        {

        }

        internal readonly MongoDatabase Database;
        readonly MongoServer mServer;
    }
}
