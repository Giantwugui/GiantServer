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
                int appId = int.Parse(args[0]);
                int subId = args.Length == 2 ? int.Parse(args[1]) : 0;

                Service.Instacne.Init(AppyType.Gate, appId, subId);

                Logger.Info($"server start complete------------- appId {appId} subId {subId}");
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
