using Giant.Core;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class HateComponent : InitSystem<Unit>, IUpdate
    {
        private Dictionary<int, int> hateList = new Dictionary<int, int>();
        public Dictionary<int, int> HateList { get { return hateList; } }

        public override void Init(Unit unit)
        {
        }

        public void Update(double dt) 
        {
        }
    }
}
