using MongoDB.Bson.Serialization.Attributes;

namespace Server.Gate
{
    [BsonIgnoreExtraElements]
    public class PlayerInfo
    {
        [BsonId]
        public long Uid { get; set; }

        public string Account { get; set; }

        public int Level { get; set; }

        public int Year = 0;

        public static int MM;

        private string PrivateStr = string.Empty;
    }
}
