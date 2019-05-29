using Giant.Log;
using Giant.Frame;
using System.Threading;
using System;
using Giant.Share;

namespace Server.App
{
    class Program : BaseProgram
    {
        static void Main(string[] args)
        {
            try
            {
                AppType appType = (AppType)Enum.Parse(typeof(AppType), args[0]);
                int appId = int.Parse(args[1]);
                int subId = args.Length == 3 ? int.Parse(args[2]) : 0;

                Service.Instacne.Init(appType, appId, subId);

                Logger.Info($"server start complete------------- appType {appType} appId {appId} subId {subId}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            while (true)
            {
                Thread.Sleep(1);


                Service.Instacne.Update();
            }
        }
       
    }
}
