using Giant.Core;
using Giant.EnumUtil;

namespace Giant.Model
{
    public class BuffModel : IData
    {
        public int Id { get; private set; }
        public BuffType BuffType { get; private set; }
        public int Value { get; private set; }
        public int DuringTime { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            BuffType = (BuffType)data.GetInt("model");
            Value = data.GetInt("Value");
            DuringTime = data.GetInt("DuringTime");
        }
    }
}
