using Giant.Core;
using Giant.Framework;

namespace Server.Account
{
    [Event(EventType.AffterFrontend)]
    public class AffterFrontendEvent : Event<FrontendComponent>
    {
        public override void Handle(FrontendComponent frontend)
        {

        }
    }
}
