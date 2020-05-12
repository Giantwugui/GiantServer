using Giant.Core;

namespace Giant.Battle
{
    public partial class PlayerUnit : Unit, IInitSystem<MapScene, UnitInfo, IBattleMsgSource, IBattleMsgListener>
    {
        public void Init(MapScene mapScene, UnitInfo info, IBattleMsgSource source, IBattleMsgListener listener)
        {
            base.Init(mapScene, UnitType.Player);

            MsgSource = source;
            MsgListener = listener;
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        protected override bool IsAny(Unit unit)
        {
            return false;
        }

        protected override bool IsEnemy(Unit unit)
        {
            return false;
        }

        protected override bool IsAutoAI()
        {
            return false;
        }
    }
}
