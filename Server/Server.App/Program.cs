using CommandLine;
using Giant.Frame;
using Giant.Log;
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
                AppOption option = null;
                Parser.Default.ParseArguments<AppOption>(args)
                    .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
                    .WithParsed(options => { option = options; });


                AppService.Instacne.Init(option);

                Logger.Info($"server start complete------------- appType {option.AppType} appId {option.AppId}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            while (true)
            {
                Thread.Sleep(1);

                AppService.Instacne.Update();
            }
        }
       
    }
}
