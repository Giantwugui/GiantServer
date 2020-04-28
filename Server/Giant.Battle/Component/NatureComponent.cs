using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class NatureComponent : InitSystem<List<Nature>>
    {
        private Unit owner => GetParent<Unit>();

        private Dictionary<NatureType, Nature> numericals;
        public Dictionary<NatureType, Nature> Numericals => numericals;

        public override void Init(List<Nature> numerical)
        {
            numericals = new Dictionary<NatureType, Nature>();
        }

        public void Add(Nature numerical)
        {
            Nature existNumerical = GetNumerical(numerical.NumericalType);
            if (existNumerical == null)
            {
                numericals.Add(numerical.NumericalType, numerical);
                return;
            }

            existNumerical.AddValue(numerical.Value);
        }

        public void Add(List<Nature> numerical)
        {
            numerical?.ForEach(x => Add(x));
        }

        public int AddValue(NatureType type, int value)
        {
            int changedValue = 0;
            Nature numerical = GetNumerical(type);
            if (numerical == null)
            {
                //减少不存在的值
                if (value < 0) return 0;

                numerical = AddComponentWithParent<Nature, NatureType, float>(type, value);
            }
            else
            {
                changedValue = numerical.AddValue(value);
            }

            owner.NumericalChange(type, value);
            return changedValue;
        }

        public Nature GetNumerical(NatureType type)
        {
            numericals.TryGetValue(type, out var numerical);
            return numerical;
        }
    }
}
