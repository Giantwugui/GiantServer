using Giant.Core;
using Giant.Data;
using System;

namespace Giant.Battle
{
    public partial class BattleScene : MapScene, IInitSystem<MapModel>, IUpdate
    {
        protected DateTime StopTime { get; private set; }

        public DungeonModel DungeonModel { get; private set; }


        public override void Init(MapModel model) 
        {
            base.Init(model);
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            CheckStart();
            CheckStop();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void CheckStart()
        {
            if (StartTime >= DungeonModel.DelayTime)
            {
                OnStart();

                StopTime = TimeHelper.Now.AddSeconds(DungeonModel.DuringTime);
            }
        }

        private void CheckStop()
        {
            if (TimeHelper.Now >= StopTime)
            {
                OnStop(BattleResult.Default);
            }
        }
    }
}
