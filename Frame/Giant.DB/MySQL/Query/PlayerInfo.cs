namespace Giant.DB
{
    public class LevelInfo
    { 
        public int Level { get; set; }
    }

    public class PlayerInfo
    {
        private LevelInfo levelInfo = new LevelInfo();

        public int Level 
        {
            get => levelInfo.Level;
            set { levelInfo.Level = value; } 
        }

        public long Uid { get; set; }

        public string Account { get; set; }

        public int Year = 0;
    }
}