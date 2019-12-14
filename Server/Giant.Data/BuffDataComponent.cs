using Giant.Core;

namespace Giant.Data
{
    public class BuffDataComponent : SingleDataComponent<BuffDataComponent, BuffModel>
    {
        public override void Load()
        {
            Load("Buff");
        }
    }
}
