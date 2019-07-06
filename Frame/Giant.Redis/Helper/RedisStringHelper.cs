using Giant.Share;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Giant.Redis
{
    public class RedisStringHelper : RedisHelper
    {
        public static RedisStringHelper Instance { get; } = new RedisStringHelper();

        private RedisStringHelper() : base()
        {
        }

        #region 同步方法

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <param name="key">保存的Key名称</param>
        /// <param name="value">对象实体</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan expiry = default)
        {
            return DataBase.StringSet(key, value);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">保存的Key名称</param>
        /// <param name="value">对象实体</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T value, TimeSpan expiry = default)
        {
            return DataBase.StringSet(key, value.ToJson());
        }

        /// <summary>
        /// 添加多个key/value
        /// </summary>
        /// <param name="valueList">key/value集合</param>
        /// <returns></returns>
        public bool StringSet(Dictionary<string, string> valueList)
        {
            return base.DataBase.StringSet(valueList.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToArray());
        }

        /// <summary>
        /// 在原有key的value值之后追加value
        /// </summary>
        /// <param name="key">追加的Key名称</param>
        /// <param name="value">追加的值</param>
        /// <returns></returns>
        public long StringAppend(string key, string value)
        {
            return base.DataBase.StringAppend(key, value);
        }

        /// <summary>
        /// 获取单个key的value值
        /// </summary>
        /// <param name="key">要获取值的Key集合</param>
        /// <returns></returns>
        public string StringGet(string key)
        {
            return base.DataBase.StringGet(key);
        }

        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public List<string> StringGet(params string[] keys)
        {
            var values = base.DataBase.StringGet(ConvertToRedisKeys(keys));
            return ConvertToString(values);
        }

        /// <summary>
        /// 获取单个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="key">要获取值的Key集合</param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        {
            return StringGet(key).FromJson<T>();
        }

        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public List<T> StringGet<T>(params string[] keys)
        {
            var values = base.DataBase.StringGet(ConvertToRedisKeys(keys));
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取旧值赋上新值
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public string StringGetSet(string key, string value)
        {
            return base.DataBase.StringGetSet(key, value);
        }

        /// <summary>
        /// 获取旧值赋上新值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public T StringGetSet<T>(string key, T value)
        {
            string oValue = base.DataBase.StringGetSet(key, value.ToJson());
            return oValue.FromJson<T>();
        }

        /// <summary>
        /// 获取值的长度
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <returns></returns>
        public long StringGetLength(string key)
        {
            return base.DataBase.StringLength(key);
        }

        /// <summary>
        /// 数字增长val，返回自增后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncrement(string key, double val = 1)
        {
            return base.DataBase.StringIncrement(key, val);
        }

        /// <summary>
        /// 数字减少val，返回自减少的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecrement(string key, double val = 1)
        {
            return base.DataBase.StringDecrement(key, val);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 异步方法 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default)
        {
            return await base.DataBase.StringSetAsync(key, value, expiry);
        }

        /// <summary>
        /// 异步方法 添加多个key/value
        /// </summary>
        /// <param name="valueList">key/value集合</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(Dictionary<string, string> valueList)
        {
            var newkeyValues = valueList.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToArray();
            return await base.DataBase.StringSetAsync(newkeyValues);
        }

        /// <summary>
        /// 异步方法 保存一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">保存的Key名称</param>
        /// <param name="obj">对象实体</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default)
        {
            return await base.DataBase.StringSetAsync(key, obj.ToJson(), expiry);
        }

        /// <summary>
        /// 异步方法 在原有key的value值之后追加value
        /// </summary>
        /// <param name="key">追加的Key名称</param>
        /// <param name="value">追加的值</param>
        /// <returns></returns>
        public async Task<long> StringAppendAsync(string key, string value)
        {
            return await base.DataBase.StringAppendAsync(key, value);
        }

        /// <summary>
        /// 异步方法 获取单个key的值
        /// </summary>
        /// <param name="key">要读取的Key名称</param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key)
        {
            return await base.DataBase.StringGetAsync(key);
        }

        /// <summary>
        /// 异步方法 获取多个key的value值
        /// </summary>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public async Task<List<string>> StringGetAsync(params string[] keys)
        {
            var values = await base.DataBase.StringGetAsync(ConvertToRedisKeys(keys));
            return values.Select(o => o.ToString()).ToList();
        }

        /// <summary>
        /// 异步方法 获取单个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="key">要获取值的Key集合</param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string key)
        {
            string values = await base.DataBase.StringGetAsync(key);
            return values.FromJson<T>();
        }

        /// <summary>
        /// 异步方法 获取多个key的value值
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="keys">要获取值的Key集合</param>
        /// <returns></returns>
        public async Task<List<T>> StringGetAsync<T>(params string[] keys)
        {
            var values = await base.DataBase.StringGetAsync(ConvertToRedisKeys(keys));
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 异步方法 获取旧值赋上新值
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public async Task<string> StringGetSetAsync(string key, string value)
        {
            return await base.DataBase.StringGetSetAsync(key, value);
        }

        /// <summary>
        /// 异步方法 获取旧值赋上新值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">新值</param>
        /// <returns></returns>
        public async Task<T> StringGetSetAsync<T>(string key, T value)
        {
            string oValue = await base.DataBase.StringGetSetAsync(key, value.ToJson());
            return oValue.FromJson<T>();
        }

        /// <summary>
        /// 异步方法 获取值的长度
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <returns></returns>
        public async Task<long> StringGetLengthAsync(string key)
        {
            return await base.DataBase.StringLengthAsync(key);
        }

        /// <summary>
        /// 异步方法 数字增长val，返回自增后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            return await base.DataBase.StringIncrementAsync(key, val);
        }

        /// <summary>
        /// 异步方法 数字减少val，返回自减少的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            return await base.DataBase.StringDecrementAsync(key, val);
        }

        #endregion 异步方法
    }
}