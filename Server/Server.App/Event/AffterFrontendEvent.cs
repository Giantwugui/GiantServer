using Giant.Core;
using Giant.Framework;

namespace Server.App
{
    [Event(EventType.AffterFrontend)]
    public class AffterFrontendEvent : Event<FrontendComponent>
    {
        public override void Handle(FrontendComponent frontend)
        {
            switch (frontend.AppConfig.AppType)
            {
                case AppType.Account:
                    // 添加一些逻辑处理组件 eg：gate向global同步信息的组件
                    frontend.AddComponent<UpLoadGateInfoComponent, FrontendComponent>(frontend);
                    break;
            }
        }
    }
}
