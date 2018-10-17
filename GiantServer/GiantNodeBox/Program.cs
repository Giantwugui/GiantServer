using System;

namespace GiantNodeBox
{
    class Program
    {
        static void Main(string[] args)
        {
            PluginManager.StartInitPlugins();

            while (true)
            {

                Console.ReadLine();
            }
        }
    }
}
