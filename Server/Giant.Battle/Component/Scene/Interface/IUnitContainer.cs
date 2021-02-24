using System.Collections.Generic;

namespace Giant.Battle
{
    public interface IUnitContainer
    {
        IReadOnlyDictionary<int, PlayerUnit> GetPlayers();
        IReadOnlyDictionary<int, HeroUnit> GetHeroes();
        IReadOnlyDictionary<int, Monster> GetMonsters();
        IReadOnlyDictionary<int, NPC> GetNPCs();
    }
}
