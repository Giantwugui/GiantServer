using Giant.Log;
using Giant.Share;
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
                AppType appType = EnumHelper.FromString<AppType>(args[0]);
                int appId = int.Parse(args[1]);

                AppService.Instacne.Init(appType, appId);

                Logger.Info($"server start complete------------- appType {appType} appId {appId}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            while (true)
            {
                Thread.Sleep(1);


                App.AppService.Instacne.Update();
            }
        }
       
    }
}
