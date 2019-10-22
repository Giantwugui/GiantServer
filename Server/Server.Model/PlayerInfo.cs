using MongoDB.Bson.Serialization.Attributes;

namespace Server.Model
{
    [BsonIgnoreExtraElements]
    public class PlayerInfo
    {
        [BsonId]
        public long Uid { get; set; }

        public string Account { get; set; }

        public int Level { get; set; }

        public int Year = 0;

        public int MapId;

        private string PrivateStr = string.Empty;
    }
}
