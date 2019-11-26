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
        private LevelInfo levelInfo = new LevelInfo();

        [BsonId]
        public int Uid { get; set; }
        public string Account { get; set; }
        public int RoleId { get; set; }
        public int Level { get; set; }
    }
}