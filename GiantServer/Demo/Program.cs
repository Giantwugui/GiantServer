using System;

namespace Demo
{
    partial class Program
    {
         static void Main(string[] args)
        {
            try
            {
                //TestRedis();

                Mongo.TestMongo();
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
