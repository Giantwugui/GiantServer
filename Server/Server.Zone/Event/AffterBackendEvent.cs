using Giant.Core;
using Giant.Framework;

namespace Server.Zone
{
    [Event(EventType.AffterBackend)]
    public class AffterBackendEvent : Event<FrontendComponent>
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
