using Giant.Core;
using Giant.Framework;

namespace Server.Gate
{
    class Program
    {
        static void Main(string[] args)
        {
            ComponentFactory.CreateComponent<BaseProgramComponent, string[]>(args);

        }
    }
}
