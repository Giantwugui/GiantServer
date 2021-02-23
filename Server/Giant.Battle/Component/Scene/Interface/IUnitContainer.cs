using System.Collections.Generic;

namespace Giant.Battle
{
    public interface IUnitContainer
    {
        Dictionary<int, PlayerUnit> GetPlayers();
        Dictionary<int, HeroUnit> GetHeroes();
        Dictionary<int, Monster> GetMonsters();
        Dictionary<int, NPC> GetNPCs();
    }
}
