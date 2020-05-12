using Giant.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit : Entity, IInitSystem<UnitInfo, IBattleMsgSource, IBattleMsgListener>, IUpdate, IBattleAction
    {
        public int Id { get; private set; }
        public UnitType UnitType { get; private set; }
        public bool IsDead { get; private set; }
        public Vector2 Position { get; private set; }

        public virtual void Init(UnitInfo info, IBattleMsgSource source, IBattleMsgListener listener)
        {
            Id = info.Id;
            UnitType = info.UnitType;

            msgSource = source;
            msgListener = listener;

            BuffComponent = AddComponentWithParent<BuffComponent>();
            FsmComponent = AddComponentWithParent<FsmComponent>(this);
            HateComponent = AddComponentWithParent<HateComponent>(this);
            TriggeComponent = AddComponentWithParent<TriggeComponent>(this);
            NatureComponent = AddComponentWithParent<NatureComponent, List<Nature>>(info.Natures);

            SkillComponent = AddComponentWithParent<SkillComponent, List<Skill>>(info.Skills);
        }

        public virtual void Update(double dt)
        {
            UpdateInBattle(dt);
        }

        public bool Move(Vector2 vector)
        {
            Position = vector;

            //TODO视野检测，地图可达性检测

            msgSource.OnMove(this, Position);

            return true;
        }

        public virtual void OnDead()
        {
            msgSource.OnDead(this);
        }

        public virtual void OnRelive()
        {
            msgSource.OnRelive(this);
        }

        protected virtual void UpdateInBattle(double dt)
        {
            BuffComponent.Update(dt);
        }

        protected virtual bool IsAny() { return false; }
        protected virtual bool IsEnemy() { return false; }
        protected virtual bool IsAutoAI() { return false; }
    }
}
