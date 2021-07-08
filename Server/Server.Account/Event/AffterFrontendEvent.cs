using Giant.Core;
using Giant.EnumUtil;
using Giant.Framework;

namespace Server.Account
{
    [Event(EventType.InitFrontend)]
    public class AffterFrontendEvent : Event<FrontendComponent>
    {
        public override void Handle(FrontendComponent frontend)
        {

        }
    }
}
