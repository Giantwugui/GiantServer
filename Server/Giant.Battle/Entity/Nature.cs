using Giant.Core;

namespace Giant.Battle
{
    public class Nature : InitSystem<NatureType, int>
    {
        private int basicValue;
        private int basicAddValue;

        public NatureType NatureType { get; private set; }
        public int Value { get; private set; }

        public override void Init(NatureType natureType, int value)
        {
            NatureType = natureType;
            basicValue = value;
            SetValue();
        }

        public int AddValue(int value)
        {
            basicAddValue += value;
            SetValue();
            return value;
        }

        private void SetValue()
        {
            Value = basicValue + basicAddValue;
        }
    }
}
