using System;

namespace Giant.DB.MySQL
{
    /// <summary>
    /// 支持MySql数据序列化对象的基类
    /// </summary>
    public abstract class MySqlData : Object
    {
    }

    public class PlayerData : MySqlData
    {
        public long Uid { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        private int level;
        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }
    }
}
