using MongoDB.Bson.Serialization.Attributes;

namespace Giant.DB
{
    [BsonIgnoreExtraElements]
    public class Player
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
