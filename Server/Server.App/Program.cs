using Giant.Log;
using Server.Frame;
using System;
using System.Threading;

namespace Server.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AppService.Instacne.Init(args);

                Logger.Warn($"server start complete------------- appType {Framework.AppType} appId {Framework.AppId}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Console.ReadLine();
            }

            while (true)
            {
                Thread.Sleep(1);

                AppService.Instacne.Update(1*0.01f);
            }
        }
       
    }
}
