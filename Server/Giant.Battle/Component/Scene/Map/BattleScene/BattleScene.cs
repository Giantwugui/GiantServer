using Giant.Core;
using Giant.Model;
using Giant.Logger;
using Giant.Util;
using System;

namespace Giant.Battle
{
    public partial class BattleScene : MapScene, IInitSystem<int, int>, IUpdate
    {
        protected DateTime StopTime { get; private set; }

        public DungeonModel DungeonModel { get; private set; }
        public int DungeonId { get { return DungeonModel.Id; } }


        public override void Init(int mapId, int channel) 
        {
            base.Init(mapId, channel);

            InitMonster();
            InitMessageDispatcher();
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

        private void InitMonster()
        {
            //TODO 种怪逻辑

            var monsters = MonsterLibrary.Instance.GetMonsterModels(DungeonId);
            if (monsters == null)
            {
                Log.Error($"monsters error monster id {DungeonId}");
                return;
            }

            foreach (var curr in monsters)
            {
                Monster monster = ComponentFactory.Create<Monster, MonsterModel>(curr);
                AddMonster(monster);
            }
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
