using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Giant.Core;
using Giant.Model;

namespace Giant.Battle
{
    internal abstract class BaseTrigger : InitSystem<Unit>
    {
        protected Unit owner;

        public override void Init(Unit unit)
        {
            owner = unit;
        }

        public virtual bool Check()
        {
            return false;
        }
    }
}
