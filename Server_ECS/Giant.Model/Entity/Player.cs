using System;
using System.Collections.Generic;

namespace Giant.Model
{
    public class Player : Entity
    {
        public DateTime LoginTime = DateTime.Now;

        public string Account { get; set; }

        public Dictionary<BookName, Dictionary<int, Item>> ItemsDict = new Dictionary<BookName, Dictionary<int, Item>>();
    }


    public class Item
    {
        public BookName ItemType;
        public int SubType;
        public int Num;
    }


    public enum BookName
    {
        None = 0,
        Hero = 1,
        Item =2,
    }


    /// <summary>
    /// 测试用户实体模型
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
    }
}
