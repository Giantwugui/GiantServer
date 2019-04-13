using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Share
{
    public class RandomHelper
    {
        private static Random random = new Random();


        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
