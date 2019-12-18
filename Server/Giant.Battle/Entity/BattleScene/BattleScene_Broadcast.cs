using Giant.Msg;

namespace Giant.Battle
{
    partial class BattleScene
    {
        private void Broadcast_BattleStart()
        {
            Msg_ZGate_Battle_Start msg = new Msg_ZGate_Battle_Start();
            BroadCast_Msg(msg);
        }

        private void BroadCast_Msg(Google.Protobuf.IMessage message)
        {
            foreach (var kv in UnitComponent.PlayerList)
            {
                kv.Value.BroadCast(message);
            }
        }
    }
}
