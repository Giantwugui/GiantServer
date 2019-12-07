using MongoDB.Bson.Serialization.Attributes;

namespace Giant.DB
{
    public class LevelInfo
    {
        public int Level { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class PlayerInfo
    {
        [BsonId]
        public int Uid { get; set; }
        public string Account { get; set; }
        public int RoleId { get; set; }
        public int Level { get; set; }
        public int MapId { get; set; }
    }
}