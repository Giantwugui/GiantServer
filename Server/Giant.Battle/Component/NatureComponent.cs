using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class NatureComponent : InitSystem<Unit>
    {
        private Natures natures;
        public Unit Owner { get; private set; }

        public override void Init(Unit unit)
        {
            Owner = unit;
            natures = new Natures();
        }

        public void Add(Nature nature) => natures.Add(nature);
        public void Add(List<Nature> natures) => this.natures.Add(natures);
        public void SetValue(NatureType type, int value) => natures.SetValue(type, value);
        public int AddValue(NatureType type, int value) => natures.AddValue(type, value);
        public Nature GetNature(NatureType type) => natures.GetNature(type);
        public int GetNatureValue(NatureType type) => natures.GetNatureValue(type);
    }
}
