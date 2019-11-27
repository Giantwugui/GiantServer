using CommandLine;
using Giant.Core;
using Giant.DB;
using Giant.Framework;
using Giant.Net;
using Giant.Redis;
using System;
using System.Threading;
using System.Reflection;

namespace Server.App
{
    class Program
    {
        static void Main(string[] args)
        {
            AppOption appOption = null;
            Parser.Default.ParseArguments<AppOption>(args)
                .WithNotParsed(error => throw new Exception("CommandLine param error !"))
                .WithParsed(options => { appOption = options; });

            IdGenerator.AppId = appOption.AppId;

            //注册Event
            Scene.EventSystem.RegistEvent(Assembly.GetEntryAssembly());
            Scene.EventSystem.RegistEvent(typeof(NetProxyComponent).Assembly);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            //窗口事件拦截
            Scene.Pool.AddComponent<WindowsEventComponent>();

            //xml表
            Scene.Pool.AddComponent<DataComponent>();
            Scene.Pool.AddComponent<AppConfigComponent>();
            Scene.Pool.AddComponent<NetGraphComponent>();

            AppConfig appConfig = Scene.Pool.GetComponent<AppConfigComponent>().GetNetConfig(appOption.AppType);
            Scene.AppConfig = appConfig;

            //消息处理
            Scene.Pool.AddComponent<OpcodeComponent>();
            Scene.Pool.AddComponent<MessageDispatcherComponent>();

            //服务注册
            Scene.Pool.AddComponent<NetProxyComponent>();

            //DB 服务
            DBConfigComponent component = Scene.Pool.AddComponent<DBConfigComponent>();
            if (appConfig.AppType.NeedDBService())
            {
                Scene.Pool.AddComponent<DBServiceComponent, DBType, DBConfig>(DBType.MongoDB, component.DBConfig);
            }

            //Redis
            if (appConfig.AppType.NeedRedisServer())
            {
                Scene.Pool.AddComponent<RedisComponent, RedisConfig>(component.RedisConfig);
            }

            //网络
            if (!string.IsNullOrEmpty(appConfig.InnerAddress))
            {
                Scene.Pool.AddComponent<InnerNetworkComponent, NetworkType, string>(NetworkType.Tcp, appConfig.InnerAddress);
            }
            if (!string.IsNullOrEmpty(appConfig.InnerAddress))
            {
                Scene.Pool.AddComponent<OutterNetworkComponent, NetworkType, string>(NetworkType.Tcp, appConfig.OutterAddress);
            }

            //控制台监听事件
            Scene.Pool.AddComponent<ConsoleComponent>();

            //启动服务注册
            Scene.EventSystem.Handle(EventType.InitDone);

            DateTime dateTime = TimeHelper.Now;
            while (true)
            {
                OneThreadSynchronizationContext.Instance.Update();//异步回调处理

                double dt = (TimeHelper.Now - dateTime).TotalSeconds;
                dateTime = TimeHelper.Now;

                Scene.EventSystem.Update(dt);

                Thread.Sleep(1);
            }
        }
    }
}
