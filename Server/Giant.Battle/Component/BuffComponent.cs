using Giant.Core;
using Giant.Data;
using Giant.Logger;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public class BuffComponent : InitSystem<IBattleMsgSource>, IUpdate
    {
        private IBattleMsgSource msgSource;
        private readonly List<long> removeList = new List<long>();
        private readonly Dictionary<long, BaseBuff> buffs = new Dictionary<long, BaseBuff>();

        public override void Init(IBattleMsgSource msgSource)
        {
            this.msgSource = msgSource;
        }

        public bool AddBuff(int buffId)
        {
            BuffModel model = BuffLibComponent.Instance.GetModel(buffId);
            if (model == null)
            {
                Log.Warn($"have no this buff {buffId}");
                return false;
            }

            BaseBuff buff = BuffFactory.BuildBuff(this, model);
            AddBuff(buff);

            return true;
        }

        public void AddBuff(BaseBuff buff)
        {
            if (buffs.ContainsKey(buff.InstanceId))
            {
                Log.Error($"add buff repeat instanceId{buff.InstanceId} buff type {buff.BuffType} buff Id {buff.Id}");
                return;
            }

            msgSource.OnAddBuff(GetParent<Unit>(), buff.Id);

            buff.Start();
            buffs[buff.InstanceId] = buff;
        }

        public BaseBuff GetBuff(long instanceId)
        {
            buffs.TryGetValue(instanceId, out var buff);
            return buff;
        }

        public bool RemoveBuff(long instanceId)
        {
            if (buffs.TryGetValue(instanceId, out var buff))
            {
                if (!buff.IsBuffEnd)
                {
                    buff.End();
                }

                msgSource.OnRemoveBuff(GetParent<Unit>(), buff.Id);

                buff.Dispose();

                return true;
            }
            return false;
        }

        public bool InBuffState(BuffType type)
        {
            return buffs.Values.Where(x => x.BuffType == type).FirstOrDefault() != null;
        }

        public void Update(double dt)
        {
            BaseBuff buff;
            foreach (long id in buffs.Keys.ToList())
            {
                buff = GetBuff(id);
                if (buff == null || buff.IsBuffEnd)
                {
                    removeList.Add(id);
                }
                else
                {
                    buff.Update(dt);
                }
            }

            foreach (var id in removeList)
            {
                buffs.Remove(id);

                buff = GetBuff(id);
                if (buff != null && !buff.IsBuffEnd)
                {
                    buff.End();
                }

                buff?.Dispose();
            }

        }
    }
}
