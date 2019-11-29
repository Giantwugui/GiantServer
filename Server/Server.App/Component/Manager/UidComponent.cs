using Giant.Core;
using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Bson;

namespace Server.App
{
    public class UidComponent : Component, IInitSystem
    {
        private int appId;
        private int maxUid;

        public static UidComponent Instance { get; private set; }

        public void Init()
        {
            Instance = this;
            appId = Scene.AppConfig.AppId;
            LoadMaxUid();
        }

        public int GetUid()
        {
            return ++maxUid;
        }

        private async void LoadMaxUid()
        {
            var query = new MongoDBQuery<PlayerInfo>(DBName.Player, x => x.Uid > 0, 1, 0, new BsonDocument("_id", -1));
            PlayerInfo player = await query.Task();

            InitUid(player == null ? 0 : player.Uid);
        }

        private void InitUid(int uid)
        {
            //一个server支持100万个账号
            maxUid = uid != 0 ? uid : appId * 100 * 10000 + 1;//1001000001;
        }
    }
}
