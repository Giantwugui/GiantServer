using Giant.Core;
using Giant.Model;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class Skill : Entity, IInitSystem<Unit, SkillModel>
    {
        private int energy;

        public int SkillEnergy { get; private set; }
        public SkillType SkillType { get; private set; }
        public Unit Owner { get; private set; }
        public SkillModel SkillModel { get; private set; }
        public int Id => SkillModel.Id;

        public void Init(Unit unit, SkillModel model)
        {
            Owner = unit;
            SkillModel = model;
        }

        public bool CheckCast()
        {
            return SkillEnergy >= energy;
        }

        public void AddEnergy(int value)
        {
            energy += value;
        }

        public void Start()
        {
        }

        public void End()
        {
        }

        public override void Dispose()
        {
        }
    }

    internal class SkillEngineComponent : Component
    {
        private List<Skill> readySkillList = new List<Skill>();

        public void Ready(int skillId)
        {
            
        }
    }
}
