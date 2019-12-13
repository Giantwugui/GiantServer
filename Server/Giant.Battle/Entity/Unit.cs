using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class Unit : Entity, IInitSystem<UnitType, int, List<Numerical>, List<Skill>>
    {
        public int Id { get; private set; }
        public UnitType UnitType { get; private set; }


        public void Init(UnitType unitType, int id, List<Numerical> numericals, List<Skill> skills)
        {
            Id = id;
            UnitType = unitType;

            AddComponentWithParent<NumericalComponent, List<Numerical>>(this, numericals);
            AddComponentWithParent<SkillComponent, List<Skill>>(this, skills);
            AddComponentWithParent<BuffComponent>(this);
        }
    }
}
