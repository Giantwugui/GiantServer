using Giant.Core;

namespace Giant.Battle
{
    public class BaseFsm : InitSystem<Unit, FsmType>
    {
        private FsmType nextFsmType;
        private object nextFsmParam;

        private FsmType fsmType = FsmType.Base;
        public FsmType FsmType
        {
            get { return fsmType; }
            set { fsmType = value; }
        }
        public bool IsEnd { get; set; }

        public Unit Owner { get; set; }

        public override void Init(Unit unit, FsmType fsmType)
        {
            FsmType = fsmType;
            Owner = unit;
        }

        public void Start()
        { 
        }

        public void End()
        {
            IsEnd = true;
        }

        public override void Dispose()
        {
            base.Dispose();

            Owner = null;
            nextFsmParam = null;
            fsmType = FsmType.Base;
        }

        protected virtual void OnStart() { }
        protected virtual void OnEnd() { }
        protected virtual void OnUpdate() { }
    }
}
