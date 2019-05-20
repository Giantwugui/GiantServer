using Giant.Log;
using Giant.Frame;
using System.Threading;

namespace Server.App
{
    class Program : BaseProgram
    {
        static void Main(string[] args)
        {
            Service.Instacne.Init();

            Logger.Info($"server start complete------------- mainId {args[0]}");
            while (true)
            {
                Thread.Sleep(1);


                Service.Instacne.Update();
            }
        }
       
    }
}
