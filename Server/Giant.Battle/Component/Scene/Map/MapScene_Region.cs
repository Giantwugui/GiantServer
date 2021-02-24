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

        public IReadOnlyDictionary<int, PlayerUnit> GetPlayers()
        {
            return playerList;
        }

        public IReadOnlyDictionary<int, Monster> GetMonsters()
        {
            return monsterList;
        }

        public IReadOnlyDictionary<int, HeroUnit> GetHeroes()
        {
            return heroList;
        }

        public IReadOnlyDictionary<int, NPC> GetNPCs()
        {
            return npcList;
        }
    }
}
