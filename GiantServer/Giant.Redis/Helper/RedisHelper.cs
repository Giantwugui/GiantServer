using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Giant.Share;

namespace Giant.Redis
{

    public abstract class RedisHelper
    {
        protected RedisHelper()
        {
            Connection = RedisManager.Instance.Connection;

            DataBase = Connection.GetDatabase(RedisManager.Instance.DataBaseIndex);
        }

        #region 同步方式

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">要删除的key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        {
            return DataBase.KeyDelete(key);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">要删除的key集合</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(params string[] keys)
        {
            return DataBase.KeyDelete(ConvertToRedisKeys(keys));
        }

        /// <summary>
        /// 清空当前DataBase中所有Key
        /// </summary>
        public void KeyFulsh()
        {
            //直接执行清除命令
            DataBase.Execute("FLUSHDB");
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">要判断的key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return DataBase.KeyExists(key);
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            return DataBase.KeyRename(key, newKey);
        }

        /// <summary>
        /// 设置Key的过期时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            return DataBase.KeyExpire(key, expiry);
        }

        #endregion

        #region 异步方法

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">要删除的key</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> KeyDeleteAsync(string key)
        {
            return await DataBase.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">要删除的key集合</param>
        /// <returns>成功删除的个数</returns>
        public async Task<long> KeyDeleteAsync(params string[] keys)
        {
            return await DataBase.KeyDeleteAsync(ConvertToRedisKeys(keys));
        }

        /// <summary>
        /// 清空当前DataBase中所有Key
        /// </summary>
        public async Task KeyFulshAsync()
        {
            //直接执行清除命令
            await DataBase.ExecuteAsync("FLUSHDB");
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">要判断的key</param>
        /// <returns></returns>
        public async Task<bool> KeyExistsAsync(string key)
        {
            return await DataBase.KeyExistsAsync(key);
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public async Task<bool> KeyRenameAsync(string key, string newKey)
        {
            return await DataBase.KeyRenameAsync(key, newKey);
        }

        /// <summary>
        /// 设置Key的过期时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            return await DataBase.KeyExpireAsync(key, expiry);
        }

        #endregion

        /// <summary>
        /// 执行Redis事务
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public bool RedisTransaction(Action<ITransaction> act)
        {
            var tran = DataBase.CreateTransaction();
            act.Invoke(tran);
            bool committed = tran.Execute();
            return committed;
        }
        /// <summary>
        /// Redis锁
        /// </summary>
        /// <param name="act"></param>
        /// <param name="ts">锁住时间</param>
        public void RedisLockTake(Action act, TimeSpan ts)
        {
            RedisValue token = Environment.MachineName;
            string lockKey = "lock_LockTake";
            if (DataBase.LockTake(lockKey, token, ts))
            {
                try
                {
                    act();
                }
                catch (Exception ex)
                {
                    throw new Exception("未处理的异常 " + ex);
                }

                finally
                {
                    DataBase.LockRelease(lockKey, token);
                }
            }
        }

        #region 扩展方法

        protected RedisKey[] ConvertToRedisKeys(params string[] param) => param.Select(x => (RedisKey)x).ToArray();

        protected List<string> ConvertToString(params RedisValue[] param)
        {
            return param.Select(x => x.ToString()).ToList();
        }

        protected RedisValue[] ConvertRedisValue<T>(params T[] redisValues) => redisValues.Select(o => (RedisValue)o.ToJson()).ToArray();

        /// <summary>
        /// 将值反系列化成对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected List<T> ConvetList<T>(params RedisValue[] values) => values.Select(x=>((string)x).ToObject<T>()).ToList();


        #endregion

        /// <summary>
        /// 数据库对象
        /// </summary>
        protected IDatabase DataBase { get; private set; }


        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private ConnectionMultiplexer Connection { get; set; }


        /// <summary>
        /// 获取Redis事务对象
        /// </summary>
        /// <returns></returns>
        public ITransaction CreateTransaction() => DataBase.CreateTransaction();
    }


    public static class RedisValueHelper
    {
        //public static T ToObject<T>(this RedisValue redisValue)
        //{

        //}
    }
}
