using Giant.Msg;

namespace Server.App
{
    partial class Service
    {
        public override void BindResponser()
        {
            base.BindResponser();

            AddResponser(OuterOpcode.CG_TEST, Test);
        }
    }
}
