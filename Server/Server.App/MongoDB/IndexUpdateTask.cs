using Giant.Core;
using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.App
{
    [MongoDBIndex]
    public class PlayerInfoIndexUpdateTask : MongoDBTask<string>
    {
        public override async Task Run()
        {
            //标记有 BsonId 属性的字段会自动创建索引 _id_

            string currIndexName = "_id_";
            var collection = GetCollection<PlayerInfo>(DBName.Account);
            List<string> indexList = await GetIndex(collection);

            if (indexList.Contains(currIndexName))
            {
                return;
            }

            var keys = Builders<PlayerInfo>.IndexKeys.Ascending("Uid");
            var indexOptions = new CreateIndexOptions<PlayerInfo>
            {
                //PartialFilterExpression = Builders<PlayerInfo>.Filter.Gt(x => x.Uid, 0),
                Background = true,
                Unique = true
            };

            await GetCollection<PlayerInfo>(DBName.Account)?.Indexes.CreateOneAsync(new CreateIndexModel<PlayerInfo>(keys, indexOptions));
        }
    }

    [MongoDBIndex]
    public class AccountIndexUpdateTask : MongoDBTask<string>
    {
        public override async Task Run()
        {
            string currIndexName = "Account_1";
            var collection = GetCollection<AccountInfo>(DBName.Account);
            List<string> indexList = await GetIndex(collection);
            if (indexList.Contains(currIndexName))
            {
                return;
            }

            var keys = Builders<AccountInfo>.IndexKeys.Ascending("Account");
            var indexOptions = new CreateIndexOptions<AccountInfo>
            {
                //PartialFilterExpression = Builders<BsonDocument>.Filter.Gt(document => document["rating"], 5),
                //Background = true,
                //Unique = true
            };

            await GetCollection<AccountInfo>(DBName.Account)?.Indexes.CreateOneAsync(new CreateIndexModel<AccountInfo>(keys, indexOptions));
        }
    }

}
