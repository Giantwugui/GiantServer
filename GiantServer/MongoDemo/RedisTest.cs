using Giant.Redis;
using Giant.Share;

namespace MongoDemo
{
    partial class Program
    {
        static void TestRedis()
        {
            Player player = new Player() { Account = "Redis" };

            RedisManager.Instance.SetLinkParam("127.0.0.1", 6379);

            RedisManager.Instance.AddString("test", "kakkaka");

            RedisManager.Instance.HashSet("Player", player.Id.ToString(), player.ToJson());

            Player cachePlayer = RedisManager.Instance.HashGet("Player", player.Id.ToString()).ToObject<Player>();
        }
    }
}
