using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using Server.Frame;

namespace Server.Manager
{
    public class UidManager
    {
        private int appId;
        private int maxUid;
        public AppService AppService { get; private set; }

        public UidManager(AppService service)
        {
            this.appId = service.AppId;
            this.AppService = service;

            LoadMaxUid();
        }

        public int GetUid()
        {
            return ++maxUid;
        }

        private async void LoadMaxUid()
        {
            FindOptions<PlayerInfo> option = new FindOptions<PlayerInfo>()
            {
                Limit = 1,
                Sort = new BsonDocument("Uid", -1),
            };
            var query = new MongoDBQuery<PlayerInfo>(DBName.Player, x=>x.Uid>0, option);
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
