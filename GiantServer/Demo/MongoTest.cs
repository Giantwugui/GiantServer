using Giant.Model;
using MongoDB.Driver;

namespace Demo
{
    partial class Program
    {
        static void TestMongo()
        {
            MongoClient mongoClient = new MongoClient("mongodb://dbOwner:dbOwner@127.0.0.1:27017/ET");
            var dataBase = mongoClient.GetDatabase("ET");

            var collection = dataBase.GetCollection<Player>("Player");


            //collection.InsertOne(square);
            //collection.InsertOne(circle);
        }
    }
}
