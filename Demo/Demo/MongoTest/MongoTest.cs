using Giant.Model;
using Giant.Share;
using MongoDB.Driver;
using System;

namespace Demo
{
    class Mongo
    {
        public static void TestMongo()
        {
            MongoClient mongoClient = new MongoClient("mongodb://dbOwner:dbOwner@127.0.0.1:27017/ET");
            var dataBase = mongoClient.GetDatabase("ET");

            var collection = dataBase.GetCollection<Player>("Player");


            Player player = new Player() { Account = "wg0001", LoginTime = DateTime.Now };
            collection.InsertOne(player);


            //collection.InsertOne(circle);
        }


    }
}
