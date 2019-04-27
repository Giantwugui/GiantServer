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
