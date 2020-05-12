using Giant.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit : Entity, IInitSystem<UnitInfo, IBattleMsgSource, IBattleMsgListener>, IUpdate
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

            AddComponentWithParent<NatureComponent, List<Nature>>(info.Natures);
            AddComponentWithParent<SkillComponent, List<Skill>>(info.Skills);
            AddComponentWithParent<BuffComponent>();
        }

        public virtual void Update(double dt)
        {
            GetComponent<BuffComponent>().Update(dt);
        }

        public bool Move(Vector2 vector)
        {
            Position = vector;

            //TODO视野检测

            msgSource.OnMove(this, Position);

            return true;
        }
    }
}
