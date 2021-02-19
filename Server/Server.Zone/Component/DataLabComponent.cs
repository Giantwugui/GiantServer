using Giant.Core;
using Giant.Model;

namespace Server.Zone
{
    public class DataLabComponent : InitSystem<AppType>
    {
        public override void Init(AppType appType)
        {
            switch (appType)
            {
                case AppType.Zone:

                    break;
                default:
                    break;
            }
        }
    }
}
