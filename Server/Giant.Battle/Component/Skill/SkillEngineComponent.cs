using Giant.Core;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    internal class SkillEngineComponent : Component, IInitSystem<Unit>
    {
        private Unit owner;
        private List<Skill> readySkillList = new List<Skill>();


        public void Init(Unit unit)
        {
            owner = unit;
        }

        public void Ready(int skillId)
        {
            if (InReadyList(skillId)) return;

            Skill skill = owner.SkillComponent.GetSkill(skillId);
            if (skill == null) return;

            readySkillList.Add(skill);

            Sort();
        }

        public bool TryFatchOneSkill(out Skill skill)
        {
            skill = null;
            foreach (var kv in readySkillList)
            {
                if (!owner.SkillComponent.Check(kv))
                {
                    continue;
                }

                skill = kv;
                return true;
            }
            return false;
        }

        private bool InReadyList(int skillId)
        {
            return readySkillList.Where(x => x.Id == skillId).FirstOrDefault() != null;
        }

        private void Sort()
        {
            readySkillList = readySkillList.OrderByDescending(x => x.Model.Priority).ToList();
        }
    }
}
