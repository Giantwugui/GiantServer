using Giant.Core;
using Giant.Data;
using Giant.Logger;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public class BuffComponent : InitSystem, IUpdate
    {
        private readonly Dictionary<long, BaseBuff> buffs = new Dictionary<long, BaseBuff>();
        private readonly ListMap<BuffType, long> buffsType2Id = new ListMap<BuffType, long>();

        public override void Init()
        {
        }

        public void AddBuff(int buffId)
        {
            BuffModel model = BuffDataComponent.Instance.GetModel(buffId);
            if (model == null)
            {
                Log.Warn($"have no this buff {buffId}");
                return;
            }

            BaseBuff buff = BuffFactory.BuildBuff(this, model);
            AddBuff(buff);
        }

        public void AddBuff(BaseBuff buff)
        {
            if (buffs.ContainsKey(buff.InstanceId))
            {
                Log.Error($"add buff repeat instanceId{buff.InstanceId} buff type {buff.BuffType} buff Id {buff.Id}");
                return;
            }

            buff.Start();
            buffs[buff.InstanceId] = buff;
            buffsType2Id.Add(buff.BuffType, buff.InstanceId);
        }

        public BaseBuff GetBuff(long instanceId)
        {
            buffs.TryGetValue(instanceId, out var buff);
            return buff;
        }

        public void RemoveBuff(long instanceId)
        {
            if (buffs.TryGetValue(instanceId, out var buff))
            {
                buffsType2Id.Remove(buff.BuffType, buff.InstanceId);
                buff.Dispose();
            }
        }

        public bool InBuffState(BuffType type)
        {
            return buffsType2Id.ContainsKey(type);
        }

        public void Update(double dt)
        {
            BaseBuff buff;
            foreach (long id in buffs.Keys.ToList())
            {
                buff = GetBuff(id);
                buff?.Update(dt);
            }
        }
    }
}
