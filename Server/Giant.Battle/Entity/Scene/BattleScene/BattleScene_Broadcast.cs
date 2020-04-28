using Giant.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Battle
{
    public partial class BattleScene
    {
        public void BroadCastMsg(Google.Protobuf.IMessage message)
        {
            UnitComponent.UnitList.ForEach(x => x.Value.BroadCast(message));
        }

        private void BroadCastMsgExceptUnit(Google.Protobuf.IMessage message, int id)
        {
            foreach (var kv in UnitComponent.UnitList)
            {
                if (kv.Key == id) continue;

                kv.Value.BroadCast(message);
            }
        }
    }
}
