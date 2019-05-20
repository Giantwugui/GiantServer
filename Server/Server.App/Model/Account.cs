using MongoDB.Bson.Serialization.Attributes;

namespace Server.App
{
    [BsonIgnoreExtraElements]
    public class AccountInfo
    {
        [BsonId]
        public long Id { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }
    }
}
