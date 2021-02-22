using Giant.Core;
using Giant.EnumUtil;
using Giant.Model;
using Giant.Util;
using System;

namespace Giant.Battle
{
    public abstract class BaseBuff : Entity, IInitSystem<Unit, BuffModel>
    {
        protected Unit owner;

        public bool IsBuffEnd { get; set; }
        public DateTime EndTime { get; private set; }
        public BuffModel Model { get; private set; }

        public int Id => Model.Id;
        public BuffType BuffType => Model.BuffType;

        public void Init(Unit owner, BuffModel model)
        {
            this.owner = owner;
            Model = model;
            EndTime = TimeHelper.Now.AddSeconds(model.DuringTime);

            OnInit();
        }

        public void Start()
        {
            IsBuffEnd = false;

            OnStart();
        }

        public virtual void Update(double dt)
        {
            if (TimeHelper.Now >= EndTime)
            {
                End();
                return;
            }

            OnUpdate((float)dt);
        }

        public void End()
        {
            IsBuffEnd = true;

            OnEnd();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected virtual void OnInit() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate(float dt) { }
        protected virtual void OnEnd() { }

        public virtual void Reset() { }
    }
}
