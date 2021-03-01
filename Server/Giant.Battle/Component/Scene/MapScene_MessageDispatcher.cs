using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Battle
{
    partial class MapScene
    {
        protected MsgDispatchComponent msgDispatcher;

        protected void InitMessageDispatcher()
        {
            msgDispatcher = ComponentFactory.CreateComponent<MsgDispatchComponent, MapScene>(this);
        }

        public MsgDispatchComponent GetMsgDispatcher()
        {
            return msgDispatcher;
        }

        public void DispatchBattleStartMessage()
        {
            msgDispatcher?.DispatchMessage(TriggerMessageType.BattleStart);
        }
    }
}
