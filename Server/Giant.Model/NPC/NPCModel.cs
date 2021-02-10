using Giant.Core;

namespace Giant.Model
{
    public class NPCModel : IData<NPCModel>
    {
        public int Id { get; private set; }

        public void Bind(DataModel data)
        {
            Id = data.Id;
        }
    }
}
