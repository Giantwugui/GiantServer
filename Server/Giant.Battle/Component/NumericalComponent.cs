using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class NumericalComponent : InitSystem<List<Numerical>>
    {
        private Dictionary<NumericalType, Numerical> numericals;
        public Dictionary<NumericalType, Numerical> Numericals => numericals;

        public override void Init(List<Numerical> numerical)
        {
            numericals = new Dictionary<NumericalType, Numerical>();
        }

        public void Add(Numerical numerical)
        {
            Numerical existNumerical = GetNumerical(numerical.NumericalType);
            if (existNumerical == null)
            {
                numericals.Add(numerical.NumericalType, numerical);
                return;
            }

            existNumerical.AddValue(numerical.Value);
        }

        public void Add(List<Numerical> numerical)
        {
            numerical?.ForEach(x => Add(x));
        }

        public Numerical GetNumerical(NumericalType type)
        { 
            numericals.TryGetValue(type, out var numerical);
            return numerical;
        }
    }
}
