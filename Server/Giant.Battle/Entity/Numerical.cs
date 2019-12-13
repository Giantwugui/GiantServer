using Giant.Core;

namespace Giant.Battle
{
    public class Numerical : InitSystem<NumericalType, float>
    {
        private float basicValue;
        private float basicAddValue;

        public NumericalType NumericalType { get; private set; }
        public float Value { get; private set; }

        public override void Init(NumericalType numericalType, float value)
        {
            NumericalType = numericalType;
            basicValue = value;
            SetValue();
        }

        public void AddValue(float value)
        {
            basicAddValue += value;
            SetValue();
        }

        private void SetValue()
        {
            Value = basicValue + basicAddValue;
        }
    }
}
