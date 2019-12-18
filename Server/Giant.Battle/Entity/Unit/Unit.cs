using Giant.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public partial class Unit : Entity, IInitSystem<UnitType, int, List<Numerical>, List<Skill>, IBattleMsgListener>, IUpdate
    {
        public int Id { get; private set; }
        public UnitType UnitType { get; private set; }
        public bool IsDead { get; private set; }
        public Vector2 Position { get; private set; }

        public void Init(UnitType unitType, int id, List<Numerical> numericals, List<Skill> skills, IBattleMsgListener listener)
        {
            Id = id;
            UnitType = unitType;
            MsgListener = listener;

            AddComponentWithParent<NumericalComponent, List<Numerical>>(numericals);
            AddComponentWithParent<SkillComponent, List<Skill>>(skills);
            AddComponentWithParent<BuffComponent>();
        }

        public void Update(double dt)
        {
            GetComponent<BuffComponent>().Update(dt);
        }

        public bool Move(Vector2 vector)
        {
            Position = vector;
            return true;
        }
    }
}
