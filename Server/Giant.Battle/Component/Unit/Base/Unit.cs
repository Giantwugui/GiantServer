using Giant.Core;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit : Entity, IInitSystem<MapScene, UnitType>, IClientAction, IUnitEvent
    {
        public int Id { get; private set; }
        public UnitType UnitType { get; private set; }
        public bool IsDead { get; private set; }
        public Vector2 Position { get; private set; }
        public MapScene MapScene { get; private set; }

        public float Radius { get; private set; }

        public void Init(MapScene mapScene, UnitType type)
        {
            Id = mapScene.GetUnitId();

            UnitType = type;
            MapScene = mapScene;

            InitBuff();
            InitFsm();
            InitHate();
            InitTrigger();
            InitNature();
            InitSkill();

            InitMoverHandler();
        }

        public virtual void Update(double dt)
        {
            FsmComponent.Update(dt);
            TriggeComponent.Update(dt);

            if (MapScene is BattleScene)
            {
                UpdateInBattle(dt);
            }
        }

        public virtual void Dead()
        {
        }

        public virtual void Relive()
        {
        }

        protected virtual void UpdateInBattle(double dt)
        {
            BuffComponent.Update(dt);
            HateComponent.Update(dt);
        }

        protected virtual bool IsAny(Unit unit) { return false; }
        protected virtual bool IsEnemy(Unit unit) { return false; }
        protected virtual bool IsAutoAI() { return false; }
    }
}
