using Giant.Model.Helper;

namespace MongoDemo
{
    partial class Program
    {
        static void TestRedis()
        {
            Player player = new Player() { Account = "Redis" };

            RedisHelper.Instance.SetLinkParam("127.0.0.1", 6379);

            RedisHelper.Instance.AddString("test", "kakkaka");

            RedisHelper.Instance.HashSet("Player", player.Id.ToString(), player.ToJson());

            Player cachePlayer = RedisHelper.Instance.HashGet("Player", player.Id.ToString()).ToObject<Player>();
        }
    }
}
