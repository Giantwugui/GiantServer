using Giant.Core;
using Giant.Framework;

namespace Server.Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            ComponentFactory.Create<BaseProgramComponent, string[]>(args);
        }
    }
}
