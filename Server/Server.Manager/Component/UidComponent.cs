using Giant.Core;
using Giant.DB;
using Giant.DB.MongoDB;
using Giant.Model;
using MongoDB.Bson;

namespace Server.Manager
{
    public class UidComponent : InitSystem
    {
        private int appId;
        private int maxUid;

        public static UidComponent Instance { get; private set; }

        public override void Init()
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
