using System;

namespace MongoDemo
{
    partial class Program
    {
         static void Main(string[] args)
        {
            try
            {
                //TestRedis();

                TestMongo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
