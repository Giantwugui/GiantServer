using Giant.Core;
using Giant.EnumUtil;
using Giant.Framework;

namespace Server.Account
{
    [Event(EventType.InitBackend)]
    public class AfterBackendEvent : Event<FrontendComponent>
    {
        public override void Handle(FrontendComponent frontend)
        {
            switch (frontend.AppConfig.AppType)
            {
                case AppType.Gate:
                    break;
            }
        }
    }
}
