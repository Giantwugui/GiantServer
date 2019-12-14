using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class Unit : Entity, IInitSystem<UnitType, int, List<Numerical>, List<Skill>>, IUpdate
    {
        public int Id { get; private set; }
        public UnitType UnitType { get; private set; }


        public void Init(UnitType unitType, int id, List<Numerical> numericals, List<Skill> skills)
        {
            Id = id;
            UnitType = unitType;

            AddComponentWithParent<NumericalComponent, List<Numerical>>(numericals);
            AddComponentWithParent<SkillComponent, List<Skill>>(skills);
            AddComponentWithParent<BuffComponent>();
        }

        public void Update(double dt)
        {
            GetComponent<BuffComponent>().Update(dt);
        }

        public void UpdateHP(int hp)
        {
            GetComponent<NumericalComponent>().GetNumerical(NumericalType.HP).AddValue(hp);
        }
    }
}
