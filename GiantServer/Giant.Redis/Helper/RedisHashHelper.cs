using System.Collections.Generic;
using StackExchange.Redis;
using Giant.Share;
using System.Linq;
using System.Threading.Tasks;

namespace Giant.Redis
{
    /// <summary>
    /// Hash:类似dictionary，通过索引快速定位到指定元素的，耗时均等，跟string的区别在于不用反序列化，直接修改某个字段
    /// string的话要么是 001:序列化整个实体
    ///           要么是 001_name:  001_pwd: 多个key-value
    /// Hash的话，一个hashid-{key:value;key:value;key:value;}
    /// 可以一次性查找实体，也可以单个，还可以单个修改
    /// </summary>
    public class RedisHashHelper : RedisHelper
    {
        private RedisHashHelper(): base()
        {
        }

        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashExists(string key, string dataKey)
        {
            return base.DataBase.HashExists(key, dataKey);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HashSet<T>(string key, string dataKey, T t)
        {
            return base.DataBase.HashSet(key, dataKey, t.ToJson());
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashDelete(string key, string dataKey)
        {
            return base.DataBase.HashDelete(key, dataKey);
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public long HashDelete(string key, params string[] dataKeys)
        {
            var newValues = dataKeys.Select(o => (RedisValue)o).ToArray();
            return base.DataBase.HashDelete(key, newValues);
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string dataKey)
        {
            string value = base.DataBase.HashGet(key, dataKey);
            return value.ToObject<T>();
        }

        /// <summary>
        /// 数字增长val，返回自增后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double HashIncrement(string key, string dataKey, double val = 1)
        {
            return base.DataBase.HashIncrement(key, dataKey, val);
        }

        /// <summary>
        /// 数字减少val，返回自减少的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double HashDecrement(string key, string dataKey, double val = 1)
        {
            return base.DataBase.HashDecrement(key, dataKey, val);
        }

        /// <summary>
        /// 获取hashkey所有key名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] HashKeys(string key)
        {
            RedisValue[] values = base.DataBase.HashKeys(key);
            return values.Select(o => o.ToString()).ToArray();
        }

        /// <summary>
        /// 获取hashkey所有key与值，必须保证Key内的所有数据类型一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> HashGetAll<T>(string key)
        {
            var query = base.DataBase.HashGetAll(key);
            Dictionary<string, T> dic = new Dictionary<string, T>();
            foreach (var item in query)
            {
                dic.Add(item.Name, ((string)item.Value).ToObject<T>());
            }
            return dic;
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 异步方法 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            return await base.DataBase.HashExistsAsync(key, dataKey);
        }

        /// <summary>
        /// 异步方法 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            return await base.DataBase.HashSetAsync(key, dataKey, t.ToJson());
        }

        /// <summary>
        /// 异步方法 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            return await base.DataBase.HashDeleteAsync(key, dataKey);
        }

        /// <summary>
        /// 异步方法 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, params string[] dataKeys)
        {
            var newValues = dataKeys.Select(o => (RedisValue)o).ToArray();
            return await base.DataBase.HashDeleteAsync(key, newValues);
        }

        /// <summary>
        /// 异步方法 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public async Task<T> HashGetAsync<T>(string key, string dataKey)
        {
            string value = await base.DataBase.HashGetAsync(key, dataKey);
            return value.ToObject<T>();
        }

        /// <summary>
        /// 异步方法 数字增长val，返回自增后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            return await base.DataBase.HashIncrementAsync(key, dataKey, val);
        }

        /// <summary>
        /// 异步方法 数字减少val，返回自减少的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            return await base.DataBase.HashDecrementAsync(key, dataKey, val);
        }

        /// <summary>
        /// 异步方法 获取hashkey所有key名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string[]> HashKeysAsync(string key)
        {
            RedisValue[] values = await base.DataBase.HashKeysAsync(key);
            return values.Select(o => o.ToString()).ToArray();
        }

        /// <summary>
        /// 获取hashkey所有key与值，必须保证Key内的所有数据类型一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> HashGetAllAsync<T>(string key)
        {
            var query = await base.DataBase.HashGetAllAsync(key);
            Dictionary<string, T> dic = new Dictionary<string, T>();
            foreach (var item in query)
            {
                dic.Add(item.Name, ((string)item.Value).ToObject<T>());
            }
            return dic;
        }

        #endregion 异步方法


        public static RedisHashHelper Instance { get; } = new RedisHashHelper();
    }
}
