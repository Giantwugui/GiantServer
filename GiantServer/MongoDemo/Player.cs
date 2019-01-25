using Giant.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDemo
{
    class Player : Entity
    {
        public string Account { get; set; }

        private Dictionary<int, int> ItemsDict = new Dictionary<int, int>();
    }
}
