using Giant.Core;

namespace Giant.Battle
{
    public class Numerical : InitSystem<NumericalType, int>
    {
        private int basicValue;
        private int basicAddValue;

        public NumericalType NumericalType { get; private set; }
        public int Value { get; private set; }

        public override void Init(NumericalType numericalType, int value)
        {
            NumericalType = numericalType;
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
