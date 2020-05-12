using Giant.Core;

namespace Giant.Data
{
    public class NPCModel : IData<NPCModel>
    {
        public int Id { get; private set; }

        public void Bind(Core.DataModel data)
        {
            Id = data.Id;
        }
    }
}
