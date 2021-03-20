using System.Collections.Generic;

namespace Giant.Battle
{
    public interface IUnitContainer
    {
        IReadOnlyDictionary<long, PlayerUnit> GetPlayers();
        IReadOnlyDictionary<long, HeroUnit> GetHeroes();
        IReadOnlyDictionary<long, Monster> GetMonsters();
        IReadOnlyDictionary<long, NPC> GetNPCs();
        IReadOnlyDictionary<long, Robot> GetRobots();
    }
}
