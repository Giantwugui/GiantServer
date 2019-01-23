using System;

namespace GiantNodeBox
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                try
                {
                    string cmd = Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }



            }
        }
    }
}
