using System;
using System.Collections.Generic;
using System.Text;
using Giant.Share;

namespace Server.Test
{
    public class Test_ARSortMap
    {
        private static ARSortMap<int, string> map = new ARSortMap<int, string>();

        public static void Test()
        {
            GenerateData();
            DoPrint();

            DoRA();
            DoPrint();
        }



        private static void DoRA()
        {
            map.Remove(4);
            map.Remove(3);
            map.Remove(2);

            map.Add(4, "4");
            map.Add(3, "3");
            map.Add(2, "2");
        }

        private static void GenerateData()
        {
            int i = 0;
            while (i++ < 10)
            {
                map.Add(i, i.ToString());
            }
        }

        private static void DoPrint()
        {
            foreach (var kv in map)
            {
                Console.WriteLine($"key {kv.Key} value {kv.Value}");
            }
        }
    }
}
