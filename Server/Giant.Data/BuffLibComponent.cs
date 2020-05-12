using Giant.Core;

namespace Giant.Data
{
    public class BuffLibComponent : SingleDataComponent<BuffLibComponent, BuffModel>
    {
        public override void Load()
        {
            Load("Buff");
        }
    }
}
