using Giant.Core;
using Giant.Framework;

namespace Server.Zone
{
    [Event(EventType.AffterFrontend)]
    public class AffterFrontendEvent : Event<FrontendComponent>
    {
        public override void Handle(FrontendComponent frontend)
        {
            switch (frontend.AppConfig.AppType)
            {
            }
        }
    }
}
