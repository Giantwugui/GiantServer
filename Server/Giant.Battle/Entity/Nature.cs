using Giant.Core;

namespace Giant.Battle
{
    public class Nature : InitSystem<NatureType, int>
    {
        private int basicValue;
        private int addValue;
        private int valueRate;

        public NatureType NatureType { get; private set; }
        public int Value { get; private set; }

        public override void Init(NatureType natureType, int value)
        {
            NatureType = natureType;
            basicValue = value;
            SetValue();
        }

        public void SetValue(int value)
        {
            basicValue = value;
        }

        public void SetValueRate(int rate)
        {
            valueRate = rate;

            SetValue();
        }

        public int AddValue(int value)
        {
            addValue += value;
            SetValue();

            return Value;
        }

        public void AddValueRate(int value)
        {
            valueRate += value;
            SetValue();
        }

        private void SetValue()
        {
            Value = (int)((basicValue + addValue) * (1 + valueRate * 0.0001f));
        }
    }
}
