using Giant.Msg;

namespace Giant.Battle
{
    partial class Unit
    {
        public MSG_ZGC_SKILL_START BuildSkillStartMessage(Skill skill)
        {
            MSG_ZGC_SKILL_START msg = new MSG_ZGC_SKILL_START()
            {
                SkillId = skill.Id,
                CasterId = Id,
                TargetId = skill.SkillCastParam.TargetId,
                SkillPosX = skill.SkillCastParam.DestPos.x,
                SkillPosY = skill.SkillCastParam.DestPos.y,
                AngleX = skill.SkillCastParam.LookDir.x,
                AngleY = skill.SkillCastParam.LookDir.y,
            };
            return msg;
        }
    }
}
