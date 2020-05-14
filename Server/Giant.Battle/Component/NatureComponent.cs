using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class NatureComponent : InitSystem
    {
        private Unit owner => GetParent<Unit>();

        private Dictionary<NatureType, Nature> natures;
        public Dictionary<NatureType, Nature> Natures => natures;

        public override void Init()
        {
            natures = new Dictionary<NatureType, Nature>();
        }

        public void Add(Nature nature)
        {
            Nature existNumerical = GetNature(nature.NatureType);
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

        public void SetValue(NatureType type, int value)
        {
            Nature nature = GetNature(type);
            if (nature == null)
            {
                nature = AddComponentWithParent<Nature, NatureType, float>(type, value);

                Add(nature);
            }
            else
            {
                nature.SetValue(value);
            }
        }

        public int AddValue(NatureType type, int value)
        {
            int changedValue = 0;
            Nature nature = GetNature(type);
            if (nature == null)
            {
                //减少不存在的值
                if (value < 0) return 0;

                nature = AddComponentWithParent<Nature, NatureType, float>(type, value);

                Add(nature);
            }
            else
            {
                changedValue = nature.AddValue(value);
            }

            owner.NatureChange(type, value);
            return changedValue;
        }

        public Nature GetNature(NatureType type)
        {
            natures.TryGetValue(type, out var nature);
            return nature;
        }

        public int GetNatureValue(NatureType type)
        {
            return natures.TryGetValue(type, out var nature) ? nature.Value : 0;
        }
    }
}
