using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Battle
{
    public partial class BattleScene
    {
        public void Start()
        {
            OnBattleStart();
        }

        public void Stop(BattleResult result)
        {
            OnBattleStop(MapComponent.Model, result);
        }

        public void Close()
        {
            OnBattleEnd();
        }
    }
}
