using Giant.Util;
using System;

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
}
