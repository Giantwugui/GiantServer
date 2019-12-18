using System.Collections.Generic;

namespace Giant.Battle
{
    public class UnitInfo
    {
        public int Id { get; set; }
        public UnitType UnitType { get; set; }
        public List<Numerical> Numericals { get; set; }
        public List<Skill> Skills { get; set; }
    }
}
