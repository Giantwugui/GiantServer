using System.Collections.Generic;

namespace Giant.Battle
{
    partial class MapScene: IUnitContainer
    {
        public RegionManager RegionManager { get; private set; }

        public void InitRegionManager()
        {
            grid = MapModel.Grid;
            gridBig = MapModel.GridBig;
            jumpPointParam = MapModel.JpParam;
            jumpPointParamBig = MapModel.JpParamBig;

            RegionManager.Init(this, MaxX - MinX, MaxY - MinY, MinX, MinY);
        }

        public IReadOnlyDictionary<long, PlayerUnit> GetPlayers()
        {
            return playerList;
        }

        public IReadOnlyDictionary<long, Monster> GetMonsters()
        {
            return monsterList;
        }

        public IReadOnlyDictionary<long, HeroUnit> GetHeroes()
        {
            return heroList;
        }

        public IReadOnlyDictionary<long, NPC> GetNPCs()
        {
            return npcList;
        }

        public IReadOnlyDictionary<long, Robot> GetRobots()
        {
            return robotList;
        }
    }
}
