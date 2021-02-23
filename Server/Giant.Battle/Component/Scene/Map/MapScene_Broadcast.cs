using Giant.Util;

namespace Giant.Battle
{
    public partial class MapScene
    {
        public void OnUnitEnter(Unit unit)
        {
        }

        public void OnUnitLeave(Unit unit)
        {
        }

        public void BroadcastMsg(Google.Protobuf.IMessage message)
        {
            Broadcast2Players(message);
        }

        protected void Broadcast2Players(Google.Protobuf.IMessage message)
        {
            playerList.ForEach(x => x.Value.MsgListener?.BroadCastBattleMsg(message));
        }

        private void Broadcast2PlayerExcept(Google.Protobuf.IMessage message, int id)
        {
            foreach (var kv in playerList)
            {
                if (kv.Key == id) continue;

                kv.Value.MsgListener?.BroadCastBattleMsg(message);
            }
        }
    }
}
