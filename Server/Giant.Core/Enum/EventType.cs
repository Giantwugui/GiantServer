namespace Giant.Core
{
    public enum EventType
    {
        //1-1000 系统事件
        InitDone = 1,
        AffterFrontend = 2,
        AffterBackend = 3,
        CommandLine = 4,

        //1001-2000 战斗系统事件
        BattleStart = 1001,
        BattleStop = 1002,
        BattleSceneClose = 1003,

        UnitEnterScene = 1100,
        UnitLeaveScene = 1101,
        UnitCastSkill = 1102,
        UnitAddBuff = 1103,
        UnitRemoveBuff = 1104,
        PosChange = 1105,
        Damage = 1106,

        UnitDead = 1200,
        UnitRelive = 1201,
        NumbercalChange = 1202,
    }
}
