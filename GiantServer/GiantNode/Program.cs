using System;

namespace GiantNode
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PluginManager.StartPlugins();

                Console.WriteLine("All node start complete !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

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
