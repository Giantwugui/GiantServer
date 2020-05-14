using Giant.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Giant.Battle
{
    public class HateInfo
    {
        public int UnitId { get; private set; }
        public int HateValue { get; private set; }
        public DateTime LastHateTime { get; private set; }

        public HateInfo(int unitId, int hate)
        {
            LastHateTime = TimeHelper.Now;
            UnitId = unitId;
            HateValue = hate;
        }

        public void AddHate(int value)
        {
            HateValue += value;
            LastHateTime = TimeHelper.Now;
        }
    }

    public class HateComponent : InitSystem<Unit>, IUpdate
    {
        private int updateTick = 2;
        private int resetTime = 5;//超过五秒没有更新则重置
        private DateTime lastDropHate = TimeHelper.Now;

        private Dictionary<int, HateInfo> hateList = new Dictionary<int, HateInfo>();
        public Dictionary<int, HateInfo> HateList { get { return hateList; } }
        public Unit Owner { get; private set; }

        public override void Init(Unit unit)
        {
            Owner = unit;
        }

        public void AddHate(int unitId, int hate)
        {
            if (!hateList.TryGetValue(unitId, out HateInfo hateInfo))
            {
                hateInfo = new HateInfo(unitId, hate);
                hateList.Add(unitId, hateInfo);
            }
            else
            {
                hateInfo.AddHate(hate);
            }

            SortHate();
        }

        public void Update(double dt) 
        {
            if ((TimeHelper.Now - lastDropHate).TotalSeconds > resetTime)
            {
                hateList.Clear();
                return;
            }
        }

        private void SortHate()
        {
            hateList = hateList.OrderByDescending(x => x.Value.HateValue).ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
