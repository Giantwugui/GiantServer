using MongoDB.Bson.Serialization.Attributes;

namespace Server.Account
{
    [BsonIgnoreExtraElements]
    public class AccountInfo
    {
        [BsonId]
        public string Account { get; set; }

        public string Password { get; set; }
    }
}
