using System;

namespace GiantNodeBox
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PluginManager.StartPlugins();

                while (true)
                {

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
