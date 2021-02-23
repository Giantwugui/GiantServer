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

        Dictionary<int, PlayerUnit> playerList = new Dictionary<int, PlayerUnit>();
        public Dictionary<int, PlayerUnit> PlayerList => playerList;

        Dictionary<int, HeroUnit> heroList = new Dictionary<int, HeroUnit>();
        Dictionary<int, HeroUnit> HeroList => heroList;

        private Dictionary<int, NPC> npcList;
        public Dictionary<int, NPC> NpcList => npcList;

        private Dictionary<int, Monster> monsterList = new Dictionary<int, Monster>();
        public Dictionary<int, Monster> MonsterList => monsterList;


        public void Init(int index, int x, int y, int width, int height, MapScene map)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.map = map;
            npcList = new Dictionary<int, NPC>();
        }

        public void AddGameObject(Unit unit, Vector2 pos)
        {
            //Log.Write("map {0} region index {1} add obj type {2} instance id {3}", map.MapID, index, obj.type.ToString(), obj.Instance_id);
            //switch (unit.FieldObjectType)
            //{
            //    case TYPE.PC:
            //        NotifySurroundFieldObjectIn(unit, true);
            //        //Log.Warn("region index {0} add player {1} ====11 region index {2}", index, obj.Instance_id, index);
            //        playerList[unit.InstanceId] = (PlayerChar)unit;
            //        break;
            //    case TYPE.ROBOT:
            //        NotifySurroundFieldObjectIn(unit, true);
            //        robotList[unit.InstanceId] = (Robot)unit;
            //        break;
            //    case TYPE.PET:
            //        NotifySurroundFieldObjectIn(unit, true);
            //        //Log.Warn("region index {0} add pet {1} ====200 region index {2}", index, obj.Instance_id, index);
            //        petList[unit.InstanceId] = (Pet)unit;
            //        break;
            //    case TYPE.HERO:
            //        NotifySurroundFieldObjectIn(unit, true);
            //        //Log.Warn("region index {0} add pet {1} ====200 region index {2}", index, obj.Instance_id, index);
            //        heroList[unit.InstanceId] = (Hero)unit;
            //        break;
            //    case TYPE.NPC:
            //        NotifySurroundFieldObjectIn(unit, true);
            //        //Log.Warn("region index {0} add pet {1} ====200 region index {2}", index, obj.Instance_id, index);
            //        npcList[unit.InstanceId] = (NPC)unit;
            //        break;
            //    case TYPE.MONSTER:
            //        NotifySurroundFieldObjectIn(unit, true);
            //        //Log.Warn("region index {0} add pet {1} ====200 region index {2}", index, obj.Instance_id, index);
            //        monsterList[unit.InstanceId] = (Monster)unit;
            //        break;
            //    default:
            //        break;
            //}
        }

        public void RemoveGameObject(Unit unit)
        {
            ////Log.Write("map {0} region index {1} remove obj type {2} instance id {3}", map.MapID, index, obj.type.ToString(), obj.Instance_id);
            //switch (unit.FieldObjectType)
            //{
            //    case TYPE.PC:
            //        NotifySurroundFieldObjectOut(unit);
            //        //Log.Warn("region index {0} remove player {1}=====8 region index {2}", index, obj.Instance_id, index);
            //        playerList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.ROBOT:
            //        NotifySurroundFieldObjectOut(unit);
            //        robotList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.PET:
            //        NotifySurroundFieldObjectOut(unit);
            //        //Log.Warn("region index {0} remove player {1}=====8 region index {2}", index, obj.Instance_id, index);
            //        petList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.HERO:
            //        NotifySurroundFieldObjectOut(unit);
            //        //Log.Warn("region index {0} remove player {1}=====8 region index {2}", index, obj.Instance_id, index);
            //        heroList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.NPC:
            //        NotifySurroundFieldObjectOut(unit);
            //        //Log.Warn("region index {0} remove player {1}=====8 region index {2}", index, obj.Instance_id, index);
            //        npcList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.MONSTER:
            //        NotifySurroundFieldObjectOut(unit);
            //        monsterList.Remove(unit.InstanceId);
            //        break;
            //    default:
            //        break;
            //}
        }

        public void LinkNeighbor(RegionDirection diretion, Region neighbor_region)
        {
            neighborList[(int)diretion] = neighbor_region;
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
            //unit.NotifyContainerFieldObjectIn(this, true);
            //for (int i = 0; i < 8; i++)
            //{
            //    if (neighborList[i] != null)
            //    {
            //        unit.NotifyContainerFieldObjectIn(neighborList[i], true);
            //    }
            //}
        }

        public void NotifyCurRegionFieldObjectIn(Unit unit)
        {
            //unit.NotifyContainerFieldObjectIn(this, true);
        }

        private void NotifySurroundFieldObjectOut(Unit unit)
        {
            //unit.NotifyCoutainerFieldObjectOut(this);
            //for (int i = 0; i < 8; i++)
            //{
            //    if (neighborList[i] != null)
            //    {
            //        unit.NotifyCoutainerFieldObjectOut(neighborList[i]);
            //    }
            //}
        }

        public void NotifyCurRegionFieldObjectOut(Unit unit)
        {
            //unit.NotifyCoutainerFieldObjectOut(this);
        }


        public void EnterRegion(Unit unit)
        {
            //switch (unit.UnitType)
            //{
            //    case UnitType.Player:
            //        PlayerUnit player = (PlayerUnit)unit;
            //        if (player.IsMapLoadingDone == false)
            //        {
            //            Log.Warn("player {0} enter region before map loading done", player.Uid);
            //        }
            //        //Log.Warn("region index {0} add pc {1}", index, obj.Instance_id);
            //        playerList.Add(unit.InstanceId, player);
            //        break;
            //    case UnitType.Robot:
            //        break;
            //    case UnitType.Pet:
            //        break;
            //    case UnitType.Hero:
            //        heroList.Add(unit.InstanceId, (HeroUnit)unit);
            //        break;
            //    case UnitType.NPC:
            //        NpcList.Add(unit.InstanceId, (NPC)unit);
            //        break;
            //    case UnitType.Monster:
            //        MonsterList.Add(unit.InstanceId, (Monster)unit);
            //        break;
            //    default:
            //        break;
            //}
        }

        public void LeaveRegion(Unit unit)
        {
            //switch (unit.UnitType)
            //{
            //    case TYPE.PC:
            //        //Log.Warn("region index {0} remove pc {1}", index, obj.Instance_id);
            //        playerList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.ROBOT:
            //        robotList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.PET:
            //        petList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.HERO:
            //        heroList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.NPC:
            //        NpcList.Remove(unit.InstanceId);
            //        break;
            //    case TYPE.MONSTER:
            //        MonsterList.Remove(unit.InstanceId);
            //        break;
            //    default:
            //        break;
            //}
        }

        public Unit GetFieldObject(UnitType type, int instance_id)
        {
            if (instance_id == 0)
            {
                return null;
            }
            //switch (type)
            //{
            //    case TYPE.PC:
            //        PlayerChar player = null;
            //        playerList.TryGetValue(instance_id, out player);
            //        return player;
            //    case TYPE.ROBOT:
            //        Robot robot = null;
            //        robotList.TryGetValue(instance_id, out robot);
            //        return robot;
            //    case TYPE.NPC:
            //        NPC npc = null;
            //        npcList.TryGetValue(instance_id, out npc);
            //        return npc;
            //    case TYPE.GOODS:
            //        Goods goods = null;
            //        goodsList.TryGetValue(instance_id, out goods);
            //        return goods;
            //    case TYPE.PET:
            //        Pet pet = null;
            //        petList.TryGetValue(instance_id, out pet);
            //        return pet;
            //    case TYPE.HERO:
            //        Hero hero = null;
            //        heroList.TryGetValue(instance_id, out hero);
            //        return hero;
            //    case TYPE.MONSTER:
            //        Monster monster = null;
            //        MonsterList.TryGetValue(instance_id, out monster);
            //        return monster;
            //    default:
            //        return null;
            //}
            return null;
        }

        public Dictionary<int, PlayerUnit> GetPlayers()
        {
            return playerList;
        }

        public Dictionary<int, HeroUnit> GetHeroes()
        {
            return heroList;
        }

        public Dictionary<int, Monster> GetMonsters()
        {
            return monsterList;
        }

        public Dictionary<int, NPC> GetNPCs()
        {
            return npcList;
        }
    }

}