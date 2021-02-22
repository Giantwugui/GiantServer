using Giant.Core;

namespace Giant.Battle
{
    public class BaseFsm : InitSystem<Unit, FsmType>
    {
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

        public void Start(object param)
        {
            OnStart(param);
        }

        public void Update(float dt)
        {
            if (IsEnd) return;

            OnUpdate(dt);
        }

        public void End()
        {
            IsEnd = true;

            OnEnd();
        }

        public bool CanStart()
        {
            return true;
        }

        protected virtual void OnStart(object param) { }
        protected virtual void OnUpdate(float dt) { }
        protected virtual void OnEnd() { }

        public override void Dispose()
        {
            base.Dispose();

            Owner = null;
            fsmType = FsmType.Base;
        }
    }
}
