namespace Giant.EnumUtil
{
    public enum TriggerConditionCombine
    {
        And = 1,
        Or = 2
    }

    public enum TriggerMessageType
    {
        BattleStart = 1,
    }

    public enum TriggerCondition
    {
        /// <summary>
        /// 经过了一段时间
        /// </summary>
        TimeElapse = 1,
    }

    public enum TriggerHandler
    {
        /// <summary>
        /// 技能
        /// </summary>
        SkillReady = 1,
        /// <summary>
        /// 自生add Buff
        /// </summary>
        SelfAddBuff = 2,
    }
}
