using Giant.Core;

namespace Giant.Model
{
    public class BuffModel : IData<BuffModel>
    {
        public int Id { get; private set; }
        public int BuffType { get; private set; }
        public int Value { get; private set; }
        public int DuringTime { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
            BuffType = data.GetInt("model");
            Value = data.GetInt("Value");
            DuringTime = data.GetInt("DuringTime");
        }
    }
}
