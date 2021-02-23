using Giant.Core;

namespace Giant.Battle
{
    public class BaseFsm : InitSystem<Unit, FsmType>
    {
        private FsmType fsmType = FsmType.Base;
        public FsmType FsmType => fsmType;

        public bool IsEnd { get; set; }
        public Unit Owner { get; set; }

        public override void Init(Unit unit, FsmType fsmType)
        {
            Owner = unit;
            this.fsmType = fsmType;
        }

        public void Start(object param)
        {
            IsEnd = false;

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
