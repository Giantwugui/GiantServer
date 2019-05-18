using Giant.Log;
using System.Threading;

namespace Server.App
{
    class Program
    {
        static Service Service;

        static void Main(string[] args)
        {
            Service = new Service();
            Service.Init();

            Logger.Info($"server start complete------------- mainId {args[0]}");
            while (true)
            {
                Thread.Sleep(1);


                Service.Update();
            }
        }
       
    }
}
