using Giant.Logger;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    // Region相邻8个格子方向 顺时针递增
    public enum RegionDirection
    {
        REGION_N = 0,
        REGION_NE = 1,
        REGION_E = 2,
        REGION_SE = 3,
        REGION_S = 4,
        REGION_SW = 5,
        REGION_W = 6,
        REGION_NW = 7
    }

    public partial class Region : IUnitContainer
    {
        Region[] neighborList = new Region[8];
        public Region[] NeighborList
        { get { return neighborList; } }

        public int index;
        public int x;
        public int y;
        public int width;
        public int height;
        private MapScene map;

        private Dictionary<long, Monster> monsterList = new Dictionary<long, Monster>();
        private Dictionary<long, PlayerUnit> playerList = new Dictionary<long, PlayerUnit>();
        private Dictionary<long, HeroUnit> heroList = new Dictionary<long, HeroUnit>();
        private Dictionary<long, Robot> robotList = new Dictionary<long, Robot>();
        private Dictionary<long, NPC> npcList = new Dictionary<long, NPC>();

        public void Init(int index, int x, int y, int width, int height, MapScene map)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.map = map;
        }

        public void AddUnit(Unit unit, Vector2 pos)
        {
            switch (unit.UnitType)
            {
                case UnitType.Player:
                    NotifySurroundFieldObjectIn(unit, true);
                    playerList[unit.InstanceId] = (PlayerUnit)unit;
                    break;
                case UnitType.Hero:
                    NotifySurroundFieldObjectIn(unit, true);
                    heroList[unit.InstanceId] = (HeroUnit)unit;
                    break;
                case UnitType.Robot:
                    NotifySurroundFieldObjectIn(unit, true);
                    robotList[unit.InstanceId] = (Robot)unit;
                    break;
                case UnitType.NPC:
                    NotifySurroundFieldObjectIn(unit, true);
                    npcList[unit.InstanceId] = (NPC)unit;
                    break;
                case UnitType.Monster:
                    NotifySurroundFieldObjectIn(unit, true);
                    monsterList[unit.InstanceId] = (Monster)unit;
                    break;
                default:
                    break;
            }
        }

        public void RemoveGameObject(Unit unit)
        {
            switch (unit.UnitType)
            {
                case UnitType.Player:
                    NotifySurroundFieldObjectOut(unit);
                    playerList.Remove(unit.InstanceId);
                    break;
                case UnitType.Robot:
                    NotifySurroundFieldObjectOut(unit);
                    robotList.Remove(unit.InstanceId);
                    break;
                case UnitType.Hero:
                    NotifySurroundFieldObjectOut(unit);
                    heroList.Remove(unit.InstanceId);
                    break;
                case UnitType.NPC:
                    NotifySurroundFieldObjectOut(unit);
                    npcList.Remove(unit.InstanceId);
                    break;
                case UnitType.Monster:
                    NotifySurroundFieldObjectOut(unit);
                    monsterList.Remove(unit.InstanceId);
                    break;
                default:
                    break;
            }
        }

        public void LinkNeighbor(RegionDirection diretion, Region neighborRegion)
        {
            neighborList[(int)diretion] = neighborRegion;
        }

        public bool InMyRegions(Region region)
        {
            if (region == null)
            {
                return false;
            }
            if (region.index == index)
            {
                return true;
            }
            for (int i = 0; i < 8; i++)
            {
                if (neighborList[i] != null && neighborList[i].index == region.index)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsNeighbor(Region region)
        {
            if (region == null)
            {
                return false;
            }
            for (int i = 0; i < 8; i++)
            {
                if (neighborList[i] != null && neighborList[i].index == region.index)
                {
                    return true;
                }
            }
            return false;
        }

        private void NotifySurroundFieldObjectIn(Unit unit, bool isBorn = false)
        {
            unit.NotifyContainerUnitIn(this, true);
            for (int i = 0; i < 8; i++)
            {
                if (neighborList[i] != null)
                {
                    unit.NotifyContainerUnitIn(neighborList[i], true);
                }
            }
        }

        public void NotifyCurRegionFieldObjectIn(Unit unit)
        {
            unit.NotifyContainerUnitIn(this, true);
        }

        private void NotifySurroundFieldObjectOut(Unit unit)
        {
            unit.NotifyCoutainerUnitOut(this);
            for (int i = 0; i < 8; i++)
            {
                if (neighborList[i] != null)
                {
                    unit.NotifyCoutainerUnitOut(neighborList[i]);
                }
            }
        }

        public void NotifyCurRegionFieldObjectOut(Unit unit)
        {
            unit.NotifyCoutainerUnitOut(this);
        }


        public void EnterRegion(Unit unit)
        {
            switch (unit.UnitType)
            {
                case UnitType.Player:
                    PlayerUnit player = (PlayerUnit)unit;
                    //if (player.IsMapLoadingDone == false)
                    //{
                    //    Log.Warn("player {0} enter region before map loading done", player.Uid);
                    //}
                    //Log.Warn("region index {0} add pc {1}", index, obj.Instance_id);
                    playerList.Add(unit.InstanceId, player);
                    break;
                case UnitType.Robot:
                    break;
                case UnitType.Pet:
                    break;
                case UnitType.Hero:
                    heroList.Add(unit.InstanceId, (HeroUnit)unit);
                    break;
                case UnitType.NPC:
                    npcList.Add(unit.InstanceId, (NPC)unit);
                    break;
                case UnitType.Monster:
                    monsterList.Add(unit.InstanceId, (Monster)unit);
                    break;
                default:
                    break;
            }
        }

        public void LeaveRegion(Unit unit)
        {
            switch (unit.UnitType)
            {
                case UnitType.Player:
                    playerList.Remove(unit.InstanceId);
                    break;
                case UnitType.Robot:
                    robotList.Remove(unit.InstanceId);
                    break;
                case UnitType.Hero:
                    heroList.Remove(unit.InstanceId);
                    break;
                case UnitType.NPC:
                    npcList.Remove(unit.InstanceId);
                    break;
                case UnitType.Monster:
                    monsterList.Remove(unit.InstanceId);
                    break;
                default:
                    break;
            }
        }

        public Unit GetUnit(UnitType type, int instanceId)
        {
            if (instanceId == 0)
            {
                return null;
            }
            switch (type)
            {
                case UnitType.Player:
                    PlayerUnit player;
                    playerList.TryGetValue(instanceId, out player);
                    return player;
                case UnitType.Robot:
                    Robot robot;
                    robotList.TryGetValue(instanceId, out robot);
                    return robot;
                case UnitType.NPC:
                    NPC npc;
                    npcList.TryGetValue(instanceId, out npc);
                    return npc;
                case UnitType.Hero:
                    HeroUnit hero;
                    heroList.TryGetValue(instanceId, out hero);
                    return hero;
                case UnitType.Monster:
                    Monster monster;
                    monsterList.TryGetValue(instanceId, out monster);
                    return monster;
                default:
                    return null;
            }
        }

        public IReadOnlyDictionary<long, PlayerUnit> GetPlayers()
        {
            return playerList;
        }

        public IReadOnlyDictionary<long, HeroUnit> GetHeroes()
        {
            return heroList;
        }

        public IReadOnlyDictionary<long, Monster> GetMonsters()
        {
            return monsterList;
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