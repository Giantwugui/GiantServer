using Giant.EnumUtil;
using Giant.Msg;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    partial class Unit
    {
        protected Region curRegion;
        public Region CurRegion
        {
            get { return curRegion; }
        }

        public void SetCurRegion(Region region)
        {
            curRegion = region;
        }

        public void AddToAoi()
        {
            if (currentMap.MapModel.AOIType == AOIType.All)
            {
                NotifyContainerUnitIn(currentMap, true);
            }
            else
            {
                curRegion = currentMap.RegionManager.GetRegion(Position);
                if (CurRegion != null)
                {
                    CurRegion.AddUnit(this, Position);
                }
            }
        }

        public void RemoveFromAoi()
        {
            if (currentMap.AOIType == AOIType.All)
            {
                NotifyCoutainerUnitOut(currentMap);
            }
            else
            {
                if (CurRegion != null)
                {
                    CurRegion.RemoveGameObject(this);
                }
            }
        }

        public void NotifyContainerUnitIn(IUnitContainer container, bool isBorn = false)
        {
            MSG_GC_BROADCAST_INFO broadcastMsg;
            IReadOnlyDictionary<long, PlayerUnit> playerList = container.GetPlayers();
            IReadOnlyDictionary<long, HeroUnit> heroList = container.GetHeroes();
            IReadOnlyDictionary<long, Monster> monsterList = container.GetMonsters();
            IReadOnlyDictionary<long, Robot> robotList = container.GetRobots();
            //switch (UnitType)
            //{
            //    #region player
            //    case UnitType.Player:
            //        // player进入格子 
            //        //1. 需要通知player 当前格子的 其他player monster pet 等等
            //        //2. 需要通知格子内其他player我的simple info
            //        PlayerUnit pc = this as PlayerUnit;
            //        if (playerList.Count > 0)
            //        {
            //            int count = 0;
            //            MSG_GC_CHARACTER_ENTER_LIST msg = new MSG_GC_CHARACTER_ENTER_LIST();
            //            MSG_GC_CHAR_SIMPLE_INFO pcSimpleInfo = new MSG_GC_CHAR_SIMPLE_INFO();
            //            pc.GetSimpleInfo(pcSimpleInfo);
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //            broadcastMsg.CharSimpleInfo = pcSimpleInfo;

            //            foreach (var player in playerList)
            //            {
            //                //通知周围player 关于我的信息
            //                if (player.Key == pc.InstanceId)
            //                {
            //                    continue;
            //                }
            //                if (!pc.IsObserver && player.Value.NeedSyncAoiInfo())
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }

            //                // 通知我 AOI里其他player的信息
            //                if (player.Value.IsObserver)
            //                {
            //                    continue;
            //                }

            //                if (count > 30)
            //                {
            //                    pc.Write(msg);
            //                    count = 0;
            //                    msg = new MSG_GC_CHARACTER_ENTER_LIST();
            //                    MSG_GC_CHAR_SIMPLE_INFO playerSimpleInfo = new MSG_GC_CHAR_SIMPLE_INFO();
            //                    player.Value.GetSimpleInfo(playerSimpleInfo);
            //                    count++;
            //                    msg.CharacterList.Add(playerSimpleInfo);
            //                }
            //                else
            //                {
            //                    MSG_GC_CHAR_SIMPLE_INFO playerSimpleInfo = new MSG_GC_CHAR_SIMPLE_INFO();
            //                    player.Value.GetSimpleInfo(playerSimpleInfo);
            //                    count++;
            //                    msg.CharacterList.Add(playerSimpleInfo);
            //                }
            //            }
            //            if (msg.CharacterList.Count > 0)
            //            {
            //                pc.Write(msg);
            //            }
            //        }
            //        // 通知pc AOI里的robot
            //        if (robotList.Count > 0)
            //        {
            //            MSG_GC_CHARACTER_ENTER_LIST msg = new MSG_GC_CHARACTER_ENTER_LIST();
            //            foreach (var robot in robotList)
            //            {
            //                MSG_GC_CHAR_SIMPLE_INFO playerSimpleInfo = new MSG_GC_CHAR_SIMPLE_INFO();
            //                robot.Value.GetSimpleInfo(playerSimpleInfo);
            //                msg.CharacterList.Add(playerSimpleInfo);
            //            }
            //            pc.Write(msg);
            //        }

            //        // 通知pc AOI里的hero
            //        if (heroList.Count > 0)
            //        {
            //            MSG_GC_HERO_ENTER_LIST msgHeros = new MSG_GC_HERO_ENTER_LIST();
            //            int heroCount = 0;
            //            foreach (var item in heroList)
            //            {
            //                if (heroCount > 30)
            //                {
            //                    pc.Write(msgHeros);
            //                    heroCount = 0;
            //                    if (item.Value.Owner != null)
            //                    {
            //                        msgHeros = new MSG_GC_HERO_ENTER_LIST();
            //                        MSG_ZGC_HERO_SIMPLE_INFO heroInfo = new MSG_ZGC_HERO_SIMPLE_INFO();
            //                        item.Value.GetSimpleInfo(heroInfo);
            //                        heroCount++;
            //                        msgHeros.HeroList.Add(heroInfo);
            //                    }
            //                }
            //                else
            //                {
            //                    if (item.Value.Owner != null)
            //                    {
            //                        MSG_ZGC_HERO_SIMPLE_INFO heroInfo = new MSG_ZGC_HERO_SIMPLE_INFO();
            //                        item.Value.GetSimpleInfo(heroInfo);
            //                        heroCount++;
            //                        msgHeros.HeroList.Add(heroInfo);
            //                    }
            //                }
            //            }
            //            if (msgHeros.HeroList.Count > 0)
            //            {
            //                pc.Write(msgHeros);
            //            }
            //        }

            //        // 通知pc AOI的monster
            //        if (monsterList.Count > 0)
            //        {
            //            MSG_GC_MONSTER_ENTER_LIST msgMons = new MSG_GC_MONSTER_ENTER_LIST();
            //            int monsterCount = 0;
            //            foreach (var item in monsterList)
            //            {
            //                // 通知pc 格子里的pet
            //                if (monsterCount > 40)
            //                {
            //                    pc.Write(msgMons);
            //                    monsterCount = 0;

            //                    msgMons = new MSG_GC_MONSTER_ENTER_LIST();
            //                    MSG_ZGC_MONSTER_SIMPLE_INFO monInfo = new MSG_ZGC_MONSTER_SIMPLE_INFO();
            //                    item.Value.GetSimpleInfo(monInfo);
            //                    monsterCount++;
            //                    msgMons.MonList.Add(monInfo);

            //                }
            //                else
            //                {
            //                    MSG_ZGC_MONSTER_SIMPLE_INFO monInfo = new MSG_ZGC_MONSTER_SIMPLE_INFO();
            //                    item.Value.GetSimpleInfo(monInfo);
            //                    monsterCount++;
            //                    msgMons.MonList.Add(monInfo);
            //                }
            //            }
            //            if (msgMons.MonList.Count > 0)
            //            {
            //                pc.Write(msgMons);
            //            }
            //        }

            //        //if (npcList.Count > 0)
            //        //{
            //        //    MSG_GC_NPC_ENTER_LIST msgNpcs = new MSG_GC_NPC_ENTER_LIST();
            //        //    foreach (var item in npcList)
            //        //    {
            //        //        // 通知pc 格子里的npc
            //        //        MSG_ZGC_NPC_SIMPLE_INFO npcInfo = new MSG_ZGC_NPC_SIMPLE_INFO();
            //        //        item.Value.GetSimpleInfo(npcInfo);
            //        //        msgNpcs.NpcList.Add(npcInfo);
            //        //    }
            //        //    if (msgNpcs.NpcList.Count > 0)
            //        //    {
            //        //        pc.Write(msgNpcs);
            //        //    }
            //        //}
            //        break;
            //    #endregion
            //    case UnitType.ROBOT:
            //        {
            //            Robot robot = this as Robot;
            //            MSG_GC_CHAR_SIMPLE_INFO pcSimpleInfo = new MSG_GC_CHAR_SIMPLE_INFO();
            //            robot.GetSimpleInfo(pcSimpleInfo);
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //            broadcastMsg.CharSimpleInfo = pcSimpleInfo;
            //            if (playerList.Count > 0)
            //            {
            //                foreach (var player in playerList)
            //                {
            //                    if (player.Value.NeedSyncAoiInfo())
            //                    {
            //                        player.Value.BroadcastList.List.Add(broadcastMsg);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                CurDungeon?.BattleFpsManager.WriteBroadcastMsg(broadcastMsg);
            //            }
            //        }
            //        break;
            //    //case UnitType.PET:
            //    //    if (playerList.Count > 0)
            //    //    {
            //    //        Pet pet = this as Pet;
            //    //        MSG_ZGC_PET_SIMPLE_INFO petSimpleInfo = new MSG_ZGC_PET_SIMPLE_INFO();
            //    //        pet.GetSimpleInfo(petSimpleInfo);
            //    //        broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //    //        broadcastMsg.PetSimpleInfo = petSimpleInfo;
            //    //        foreach (var player in playerList)
            //    //        {
            //    //            if (player.Value.NeedSyncAoiInfo())
            //    //            {
            //    //                player.Value.BroadcastList.List.Add(broadcastMsg);
            //    //            }
            //    //        }
            //    //    }
            //    //    break;
            //    case UnitType.HERO:
            //        HeroUnit hero = this as HeroUnit;
            //        MSG_ZGC_HERO_SIMPLE_INFO heroSimpleInfo = new MSG_ZGC_HERO_SIMPLE_INFO();
            //        hero.GetSimpleInfo(heroSimpleInfo);
            //        broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //        broadcastMsg.HeroSimpleInfo = heroSimpleInfo;
            //        if (playerList.Count > 0)
            //        {
            //            foreach (var player in playerList)
            //            {
            //                if (player.Value.NeedSyncAoiInfo())
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            CurDungeon?.BattleFpsManager.WriteBroadcastMsg(broadcastMsg);
            //        }
            //        break;
            //    //case UnitType.NPC:
            //    //    if (playerList.Count > 0)
            //    //    {
            //    //        NPC npc = (NPC)obj;
            //    //        MSG_ZGC_NPC_SIMPLE_INFO npcSimpleInfo = new MSG_ZGC_NPC_SIMPLE_INFO();
            //    //        npc.GetSimpleInfo(npcSimpleInfo);
            //    //        //Client.BroadCastMsgMemoryMaker(petSimpleInfo, out header, out body);
            //    //        broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //    //        broadcastMsg.NpcSimpleInfo = npcSimpleInfo;
            //    //        foreach (var player in playerList)
            //    //        {
            //    //            player.Value.BroadcastList.List.Add(broadcastMsg);
            //    //        }
            //    //    }
            //    //    break;
            //    case UnitType.MONSTER:
            //        #region Monster
            //        //Monster debug 
            //        if (playerList.Count > 0)
            //        {
            //            Monster monster = this as Monster;
            //            MSG_ZGC_MONSTER_SIMPLE_INFO monsterSimpleInfo = new MSG_ZGC_MONSTER_SIMPLE_INFO();
            //            monster.GetSimpleInfo(monsterSimpleInfo);
            //            monsterSimpleInfo.Borning = isBorn;
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();

            //            broadcastMsg.MonsterInfo = monsterSimpleInfo;
            //            foreach (var player in playerList)
            //            {
            //                if (player.Value.NeedSyncAoiInfo())
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }
            //            }
            //        }
            //        #endregion
            //        break;
            //    default:
            //        break;
            //}
        }

        public void NotifyCoutainerUnitOut(IUnitContainer container)
        {
            MSG_GC_BROADCAST_INFO broadcastMsg;
            IReadOnlyDictionary<long, PlayerUnit> playerList = container.GetPlayers();
            IReadOnlyDictionary<long, HeroUnit> heroList = container.GetHeroes();
            IReadOnlyDictionary<long, Monster> monsterList = container.GetMonsters();
            IReadOnlyDictionary<long, Robot> robotList = container.GetRobots();

            //switch (FieldObjectType)
            //{
            //    #region PC
            //    case UnitType.PC:
            //        PlayerChar pc = this as PlayerChar;
            //        // 通知周围的player  我走了
            //        if (playerList.Count > 0 && !pc.IsObserver)
            //        {
            //            MSG_GC_CHARACTER_LEAVE pcLeaveMsg = new MSG_GC_CHARACTER_LEAVE();
            //            pcLeaveMsg.InstanceIdList.Add(InstanceId);
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //            broadcastMsg.CharLeave = pcLeaveMsg;
            //            foreach (var player in playerList)
            //            {
            //                if (player.Value.NeedSyncAoiInfo() && player.Key != InstanceId)
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }
            //            }
            //        }

            //        // 通知我 将AOI里的player monster 等等消除
            //        // other players 
            //        MSG_GC_INSTANCES_REMOVE instancesRemove = new MSG_GC_INSTANCES_REMOVE();
            //        foreach (var player in playerList)
            //        {
            //            if (player.Key != InstanceId && !player.Value.IsObserver)
            //            {
            //                instancesRemove.InstanceIdList.Add(player.Key);
            //            }
            //        }
            //        //robot
            //        foreach (var robot in robotList)
            //        {
            //            instancesRemove.InstanceIdList.Add(robot.Key);
            //        }
            //        // pet
            //        foreach (var item in petList)
            //        {
            //            // 有可能player 的 pet 还在这个格子，需要过几帧才能跟随主人离开 
            //            if (item.Value.Owner != null && item.Value.Owner.InstanceId != pc.InstanceId)
            //            {
            //                instancesRemove.InstanceIdList.Add(item.Key);
            //            }
            //        }
            //        //hero
            //        foreach (var item in heroList)
            //        {
            //            // 有可能player 的 hero 还在这个格子，需要过几帧才能跟随主人离开 
            //            if (item.Value.Owner != null && item.Value.Owner.InstanceId != pc.InstanceId)
            //            {
            //                instancesRemove.InstanceIdList.Add(item.Key);
            //            }
            //        }
            //        // Monster
            //        foreach (var item in monsterList)
            //        {
            //            instancesRemove.InstanceIdList.Add(item.Key);
            //        }
            //        // npc
            //        //foreach (var item in npcList)
            //        //{
            //        //    instancesRemove.InstanceIdList.Add(item.Key);
            //        //}
            //        if (instancesRemove.InstanceIdList.Count > 0)
            //        {
            //            pc.Write(instancesRemove);
            //        }

            //        break;
            //    #endregion
            //    case UnitType.ROBOT:
            //        if (playerList.Count > 0)
            //        {
            //            Robot robot = this as Robot;
            //            MSG_GC_CHARACTER_LEAVE pcLeaveMsg = new MSG_GC_CHARACTER_LEAVE();
            //            pcLeaveMsg.InstanceIdList.Add(InstanceId);
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //            broadcastMsg.CharLeave = pcLeaveMsg;
            //            foreach (var player in playerList)
            //            {
            //                if (player.Value.NeedSyncAoiInfo())
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }
            //            }
            //        }
            //        break;
            //    case UnitType.PET:
            //        if (playerList.Count > 0)
            //        {
            //            Pet pet = this as Pet;
            //            MSG_ZGC_PET_LEAVE petLeave = new MSG_ZGC_PET_LEAVE();
            //            petLeave.InstanceIdList.Add(pet.InstanceId);
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //            broadcastMsg.PetLeave = petLeave;

            //            foreach (var player in playerList)
            //            {
            //                if (player.Value.NeedSyncAoiInfo())
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }
            //            }
            //        }
            //        break;
            //    case UnitType.HERO:
            //        if (playerList.Count > 0)
            //        {
            //            Hero hero = this as Hero;
            //            MSG_ZGC_HERO_LEAVE heroLeave = new MSG_ZGC_HERO_LEAVE();
            //            heroLeave.InstanceIdList.Add(hero.InstanceId);
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //            broadcastMsg.HeroLeave = heroLeave;

            //            foreach (var player in playerList)
            //            {
            //                if (player.Value.NeedSyncAoiInfo())
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }
            //            }
            //        }
            //        break;
            //    //case UnitType.NPC:
            //    //    if (playerList.Count > 0)
            //    //    {
            //    //        NPC npc = (NPC)obj;
            //    //        MSG_ZGC_NPC_LEAVE npcLeave = new MSG_ZGC_NPC_LEAVE();
            //    //        npcLeave.InstanceIdList.Add(npc.InstanceId);
            //    //        broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //    //        broadcastMsg.NpcLeave = npcLeave;
            //    //        foreach (var player in playerList)
            //    //        {
            //    //            if (player.Value.IsLeavingMap == false)
            //    //            {
            //    //                player.Value.BroadcastList.List.Add(broadcastMsg);
            //    //            }
            //    //        }
            //    //    }
            //    //    break;
            //    case UnitType.MONSTER:
            //        if (playerList.Count > 0)
            //        {
            //            Monster monster = this as Monster;
            //            MSG_ZGC_MONSTER_LEAVE monsterLeave = new MSG_ZGC_MONSTER_LEAVE();
            //            monsterLeave.InstanceIdList.Add(monster.InstanceId, monster.IsDead);
            //            broadcastMsg = new MSG_GC_BROADCAST_INFO();
            //            broadcastMsg.MonsterLeave = monsterLeave;
            //            foreach (var player in playerList)
            //            {
            //                if (player.Value.NeedSyncAoiInfo())
            //                {
            //                    player.Value.BroadcastList.List.Add(broadcastMsg);
            //                }
            //            }
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

    }
}
