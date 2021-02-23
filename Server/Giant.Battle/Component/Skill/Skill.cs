using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;
using UnityEngine;

namespace Giant.Battle
{
    public class SkillCastParam
    {
        public Vector2 DestPos { get; set; }
        public Vector2 LookDir { get; set; }
        public int TargetId { get; set; }
    }

    public class Skill : Entity, IInitSystem<Unit, SkillModel>
    {
        private int energy;
        private int energyLimit;
        private SkillCastParam skillCastParam;

        public Unit Owner { get; private set; }
        public SkillModel Model { get; private set; }

        public int Id => Model.Id;
        public SkillType SkillType => Model.SkillType;
        public SkillCastParam SkillCastParam => skillCastParam;

        public void Init(Unit unit, SkillModel model)
        {
            Owner = unit;
            Model = model;
            energyLimit = model.Energy;
            skillCastParam = new SkillCastParam();
        }

        public bool Check()
        {
            return IsSkillEnough();
        }

        public void AddEnergy(int value)
        {
            if (energy > energyLimit) return;

            energy += value;

            if (energy > energyLimit)
            {
                energy = energyLimit;
            }
        }

        public void ResetEnergy()
        {
            energy = 0;
        }

        public bool IsSkillEnough()
        {
            return energy >= energyLimit;
        }

        public void InitSkillCastParam(Vector2 pos, Vector2 lookDir, int targetId)
        {
            skillCastParam.DestPos = pos;
            skillCastParam.LookDir = lookDir;
            skillCastParam.TargetId = targetId;
        }
    }
}
