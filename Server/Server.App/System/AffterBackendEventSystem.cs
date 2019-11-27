using Giant.Core;
using Giant.Framework;

namespace Server.App
{
    [Event(EventType.AffterBackend)]
    public class AffterBackendEventSystem : Event<FrontendComponent>
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
