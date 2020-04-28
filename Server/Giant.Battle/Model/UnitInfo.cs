using System.Collections.Generic;

namespace Giant.Battle
{
    public class UnitInfo
    {
        public int Id { get; set; }
        public UnitType UnitType { get; set; }
        public List<Nature> Natures { get; set; }
        public List<Skill> Skills { get; set; }
    }
}
