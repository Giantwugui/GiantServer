using System;
using System.Collections.Generic;
using System.Text;
using Giant.Model;
using Giant.Logger;

namespace Giant.Battle
{
    public class FsmSkill : BaseFsm
    {
        private Skill skill;
        private float elapseTime;

        protected override void OnStart(object param)
        {
            skill = param as Skill;
            if (skill == null)
            {
                Log.Error($"start fsm skill error, error param {param}");
                IsEnd = true;
                return;
            }

            Owner.SkillComponent.StartCasting(skill);
        }

        protected override void OnUpdate(float dt)
        {
            elapseTime += dt;
            if (elapseTime >= skill.Model.EffectTime)
            {
                Owner.SkillEffect(skill);
            }
        }

        protected override void OnEnd()
        {
        }
    }
}
