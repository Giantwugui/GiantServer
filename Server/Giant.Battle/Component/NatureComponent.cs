using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class NatureComponent : InitSystem<List<Nature>>
    {
        private Unit owner => GetParent<Unit>();

        private Dictionary<NatureType, Nature> natures;
        public Dictionary<NatureType, Nature> Natures => natures;

        public override void Init(List<Nature> nature)
        {
            natures = new Dictionary<NatureType, Nature>();
        }

        public void Add(Nature nature)
        {
            Nature existNumerical = GetNumerical(nature.NatureType);
            if (existNumerical == null)
            {
                natures.Add(nature.NatureType, nature);
                return;
            }

            existNumerical.AddValue(nature.Value);
        }

        public void Add(List<Nature> natures)
        {
            natures?.ForEach(x => Add(x));
        }

        public int AddValue(NatureType type, int value)
        {
            int changedValue = 0;
            Nature nature = GetNumerical(type);
            if (nature == null)
            {
                //减少不存在的值
                if (value < 0) return 0;

                nature = AddComponentWithParent<Nature, NatureType, float>(type, value);
            }
            else
            {
                changedValue = nature.AddValue(value);
            }

            owner.NatureChange(type, value);
            return changedValue;
        }

        public Nature GetNumerical(NatureType type)
        {
            natures.TryGetValue(type, out var numerical);
            return numerical;
        }
    }
}
