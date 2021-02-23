using Giant.Logger;
using Giant.Msg;

namespace Giant.Battle
{
    public class FsmSkill : BaseFsm
    {
        private Skill skill;
        private bool isStarted;
        private float delayTime;
        private float effectTime;
        private float skillLeftTime;
        private int skillEffectCount;

        protected override void OnStart(object param)
        {
            skill = param as Skill;
            if (skill == null)
            {
                Log.Error($"start fsm skill error, error param {param}");
                IsEnd = true;
                return;
            }

            delayTime = skill.Model.DelayTime;
            effectTime = skill.Model.EffectTime;
            skillLeftTime = skill.Model.DuringTime;

            CheckStart();

            Owner.SkillComponent.StartCasting(skill);
        }

        protected override void OnUpdate(float dt)
        {
            delayTime -= dt;
            effectTime -= dt;
            skillLeftTime -= dt;

            CheckStart();

            if (effectTime <= 0 && skillEffectCount > 0)
            {
                SkillEffect();
            }

            if (skillLeftTime <= 0)
            {
                IsEnd = true;
            }
        }

        protected override void OnEnd()
        {
        }

        private void CheckStart()
        {
            if (isStarted) return;

            if (delayTime <= 0)
            {
                isStarted = true;
            }

            Owner.OnSkillStart(skill);
        }

        private void SkillEffect()
        { 
            Owner.SkillEffect(skill);
            skillEffectCount--;
        }
    }
}
