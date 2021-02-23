using Giant.Core;

namespace Giant.Battle
{
    public partial class PlayerUnit : Unit, IInitSystem<MapScene, UnitInfo, IBattleMsgListener>
    {
        private UnitInfo unitInfo;

        public void Init(MapScene mapScene, UnitInfo info, IBattleMsgListener listener)
        {
            base.Init(mapScene, UnitType.Player);

            unitInfo = info;

            MsgListener = listener;
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        protected override void InitNature()
        {
            NatureComponent.Add(unitInfo?.Natures);
        }

        protected override bool IsAny(Unit unit)
        {
            switch (unit.UnitType)
            {
                case UnitType.Player:
                case UnitType.Hero:
                    return true;
                case UnitType.Monster:
                    return false;
            }
            return false;
        }

        protected override bool IsEnemy(Unit unit)
        {
            switch (unit.UnitType)
            {
                case UnitType.Player:
                case UnitType.Hero:
                    return false;
                case UnitType.Monster:
                    return true;
            }
            return false;
        }

        protected override bool IsAutoAI()
        {
            return false;
        }
    }
}
