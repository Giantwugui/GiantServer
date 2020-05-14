using System.Collections.Generic;

namespace Giant.Core
{
    public class Natures
    {
        private Dictionary<NatureType, Nature> natureList = new Dictionary<NatureType, Nature>();
        public Dictionary<NatureType, Nature> natuerList => natureList;

        public void Add(Nature nature)
        {
            Nature existNumerical = GetNature(nature.NatureType);
            if (existNumerical == null)
            {
                natureList.Add(nature.NatureType, nature);
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
                nature = new Nature(type, value);

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

                nature = new Nature(type, value);

                Add(nature);
            }
            else
            {
                changedValue = nature.AddValue(value);
            }

            return changedValue;
        }

        public Nature GetNature(NatureType type)
        {
            natureList.TryGetValue(type, out var nature);
            return nature;
        }

        public int GetNatureValue(NatureType type)
        {
            return natureList.TryGetValue(type, out var nature) ? nature.Value : 0;
        }
    }
}
