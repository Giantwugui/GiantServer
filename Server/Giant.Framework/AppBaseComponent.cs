using CommandLine;
using Giant.Core;
using Giant.DB;
using Giant.Net;
using Giant.Redis;
using System;
using System.Reflection;
using System.Threading;

namespace Giant.Framework
{
    public class AppBaseComponent : IInitSystem<string[]>
    {
        public void Init(string[] args)
        {
            AppOption appOption = null;
            Parser.Default.ParseArguments<AppOption>(args)
                .WithNotParsed(error => throw new Exception("CommandLine param error !"))
                .WithParsed(options => { appOption = options; });

            IdGenerator.AppId = appOption.AppId;

            //注册Event
            Scene.EventSystem.Add(Assembly.GetEntryAssembly());
            Scene.EventSystem.Add(typeof(NetProxyComponent).Assembly);

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            //xml表
            Scene.Pool.AddComponentWithCreate<DataComponent>();
            Scene.Pool.AddComponentWithCreate<AppConfigComponent>();
            Scene.Pool.AddComponentWithCreate<NetGraphComponent>();

            Scene.AppConfig = Scene.Pool.GetComponent<AppConfigComponent>().GetNetConfig(appOption.AppType);

            Scene.Pool.AddComponentWithCreate<WindowsEventComponent>();
            Scene.Pool.AddComponentWithCreate<OpcodeComponent>();
            Scene.Pool.AddComponentWithCreate<MessageDispatcherComponent>();
            Scene.Pool.AddComponentWithCreate<TimerComponent>();
            Scene.Pool.AddComponentWithCreate<NetProxyComponent>();
            Scene.Pool.AddComponentWithCreate<ConsoleComponent>();

            DBConfigComponent component = Scene.Pool.AddComponentWithCreate<DBConfigComponent>();
            if (Scene.AppConfig.AppType.NeedDBService())
            {
                Scene.Pool.AddComponentWithCreate<DBServiceComponent, DBType, DBConfig>(DBType.MongoDB, component.DBConfig);
            }

            //Redis
            if (Scene.AppConfig.AppType.NeedRedisServer())
            {
                Scene.Pool.AddComponentWithCreate<RedisComponent, RedisConfig>(component.RedisConfig);
            }

            //网络
            if (!string.IsNullOrEmpty(Scene.AppConfig.InnerAddress))
            {
                Scene.Pool.AddComponentWithCreate<InnerNetworkComponent, NetworkType, string>(NetworkType.Tcp, Scene.AppConfig.InnerAddress);
            }
            if (!string.IsNullOrEmpty(Scene.AppConfig.OutterAddress))
            {
                Scene.Pool.AddComponentWithCreate<OutterNetworkComponent, NetworkType, string>(NetworkType.Tcp, Scene.AppConfig.OutterAddress);
            }
        }
    }
}
