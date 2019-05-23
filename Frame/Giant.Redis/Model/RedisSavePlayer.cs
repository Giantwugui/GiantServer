using Giant.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Giant.Redis
{
    public class RedisSavePlayer : RedisTask<bool>
    {
        private readonly PlayerInfo playerInfo;

        public RedisSavePlayer(PlayerInfo playerInfo)
        {
            this.playerInfo = playerInfo;
        }

        public override async Task Run()
        {
            try
            {
                string redisKey = $"P:{playerInfo.Uid}";

                List<HashEntry> entries = new List<HashEntry>
                {
                    new HashEntry("Uid", playerInfo.Uid),
                    new HashEntry("Account", playerInfo.Account),
                    new HashEntry("Level", playerInfo.Level)
                };

                await this.Database.HashSetAsync(redisKey, entries.ToArray());

                SetResult(true);
            }
            catch (Exception ex)
            {
                SetException(ex);
                Logger.Error(ex);
            }
        }
    }
}
