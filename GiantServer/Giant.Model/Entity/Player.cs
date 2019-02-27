using System;
using System.Collections.Generic;

namespace Giant.Model
{
    public class Player : Entity
    {
        public DateTime LoginTime = DateTime.Now;

        public string Account { get; set; }

        private Dictionary<int, int> ItemsDict = new Dictionary<int, int>();
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
