using Giant.Core;
using Giant.Data;

namespace Giant.Battle
{
    public abstract class BaseBuff : Entity
    {
        protected Unit owner;

        public BuffModel Model { get; private set; }

        public void Init(BuffModel model)
        {
            Model = model;
            owner = GetParent<BuffComponent>().GetParent<Unit>();
        }

        public virtual void Start()
        { 
        }

        public virtual void End()
        {
        }

        public virtual void Reset() { }
        public virtual void Update() { }
    }
}
