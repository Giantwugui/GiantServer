using Giant.Core;
using Giant.Util;
using System.Linq;

namespace Giant.Battle
{
    public partial class MapScene
    {
        public void BroadcastMsg(Google.Protobuf.IMessage message)
        {
            Broadcast2Players(message);
        }

        protected void Broadcast2Players(Google.Protobuf.IMessage message)
        {
            playerList.ForEach(x => x.Value.Broadcast(message));
        }

        private void Broadcast2PlayerExcept(Google.Protobuf.IMessage message, int id)
        {
            foreach (var kv in playerList)
            {
                if (kv.Key == id) continue;

                kv.Value.Broadcast(message);
            }
        }
    }
}
