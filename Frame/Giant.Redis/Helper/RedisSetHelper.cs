using Giant.Share;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.Redis
{
    /// <summary>
    /// Set：用哈希表来保持字符串的唯一性，没有先后顺序，存储一些集合性的数据
    /// 1.共同好友、二度好友
    /// 2.利用唯一性，可以统计访问网站的所有独立 IP
    /// </summary>
    public class RedisSetHelper : RedisHelper
    {
        public static RedisSetHelper Instance { get; } = new RedisSetHelper();

        private RedisSetHelper() : base()
        {
        }

        #region 同步方法

        /// <summary>
        /// 在Key集合中添加一个value值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetAdd<T>(string key, T value)
        {
            return base.DataBase.SetAdd(key, value.ToJson());
        }

        /// <summary>
        /// 在Key集合中添加多个value值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">值列表</param>
        /// <returns></returns>
        public long SetAdd<T>(string key, List<T> value)
        {
            RedisValue[] valueList = base.ConvertRedisValue(value.ToArray());
            return base.DataBase.SetAdd(key, valueList);
        }

        /// <summary>
        /// 获取key集合值的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SetLength(string key)
        {
            return base.DataBase.SetLength(key);
        }

        /// <summary>
        /// 判断Key集合中是否包含指定的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key"></param>
        /// <param name="value">要判断是值</param>
        /// <returns></returns>
        public bool SetContains<T>(string key, T value)
        {
            return base.DataBase.SetContains(key, value.ToJson());
        }

        /// <summary>
        /// 随机获取key集合中的一个值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T SetRandomMember<T>(string key)
        {
            string rValue = base.DataBase.SetRandomMember(key);
            return rValue.FromJson<T>();
        }

        /// <summary>
        /// 获取key所有值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> SetMembers<T>(string key)
        {
            var rValue = base.DataBase.SetMembers(key);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 删除key集合中指定的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long SetRemove<T>(string key, params T[] value)
        {
            RedisValue[] valueList = base.ConvertRedisValue(value);
            return base.DataBase.SetRemove(key, valueList);
        }

        /// <summary>
        /// 随机删除key集合中的一个值，并返回该值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T SetPop<T>(string key)
        {
            string rValue = base.DataBase.SetPop(key);
            return rValue.FromJson<T>();
        }

        /// <summary>
        /// 获取几个集合的并集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public List<T> SetCombineUnion<T>(params string[] keys)
        {
            return _SetCombine<T>(SetOperation.Union, keys);
        }

        /// <summary>
        /// 获取几个集合的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public List<T> SetCombineIntersect<T>(params string[] keys)
        {
            return _SetCombine<T>(SetOperation.Intersect, keys);
        }

        /// <summary>
        /// 获取几个集合的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public List<T> SetCombineDifference<T>(params string[] keys)
        {
            return _SetCombine<T>(SetOperation.Difference, keys);
        }

        /// <summary>
        /// 获取几个集合的并集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public long SetCombineUnionAndStore(string destination, params string[] keys)
        {
            return _SetCombineAndStore(SetOperation.Union, destination, keys);
        }

        /// <summary>
        /// 获取几个集合的交集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public long SetCombineIntersectAndStore(string destination, params string[] keys)
        {
            return _SetCombineAndStore(SetOperation.Intersect, destination, keys);
        }

        /// <summary>
        /// 获取几个集合的差集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public long SetCombineDifferenceAndStore(string destination, params string[] keys)
        {
            return _SetCombineAndStore(SetOperation.Difference, destination, keys);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 在Key集合中添加一个value值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool> SetAddAsync<T>(string key, T value)
        {
            return await base.DataBase.SetAddAsync(key, value.ToJson());
        }

        /// <summary>
        /// 在Key集合中添加多个value值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key名称</param>
        /// <param name="value">值列表</param>
        /// <returns></returns>
        public async Task<long> SetAddAsync<T>(string key, List<T> value)
        {
            RedisValue[] valueList = base.ConvertRedisValue(value.ToArray());
            return await base.DataBase.SetAddAsync(key, valueList);
        }

        /// <summary>
        /// 获取key集合值的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> SetLengthAsync(string key)
        {
            return await base.DataBase.SetLengthAsync(key);
        }

        /// <summary>
        /// 判断Key集合中是否包含指定的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key"></param>
        /// <param name="value">要判断是值</param>
        /// <returns></returns>
        public async Task<bool> SetContainsAsync<T>(string key, T value)
        {
            return await base.DataBase.SetContainsAsync(key, value.ToJson());
        }

        /// <summary>
        /// 随机获取key集合中的一个值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> SetRandomMemberAsync<T>(string key)
        {
            string rValue = await base.DataBase.SetRandomMemberAsync(key);
            return rValue.FromJson<T>();
        }

        /// <summary>
        /// 获取key所有值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> SetMembersAsync<T>(string key)
        {
            var rValue = await base.DataBase.SetMembersAsync(key);
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 删除key集合中指定的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> SetRemoveAsync<T>(string key, params T[] value)
        {
            RedisValue[] valueList = base.ConvertRedisValue(value);
            return await base.DataBase.SetRemoveAsync(key, valueList);
        }

        /// <summary>
        /// 随机删除key集合中的一个值，并返回该值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> SetPopAsync<T>(string key)
        {
            string rValue = await base.DataBase.SetPopAsync(key);
            return rValue.FromJson<T>();
        }

        /// <summary>
        /// 获取几个集合的并集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<List<T>> SetCombineUnionAsync<T>(params string[] keys)
        {
            return await _SetCombineAsync<T>(SetOperation.Union, keys);
        }

        /// <summary>
        /// 获取几个集合的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<List<T>> SetCombineIntersectAsync<T>(params string[] keys)
        {
            return await _SetCombineAsync<T>(SetOperation.Intersect, keys);
        }

        /// <summary>
        /// 获取几个集合的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<List<T>> SetCombineDifferenceAsync<T>(params string[] keys)
        {
            return await _SetCombineAsync<T>(SetOperation.Difference, keys);
        }

        /// <summary>
        /// 获取几个集合的并集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<long> SetCombineUnionAndStoreAsync(string destination, params string[] keys)
        {
            return await _SetCombineAndStoreAsync(SetOperation.Union, destination, keys);
        }

        /// <summary>
        /// 获取几个集合的交集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<long> SetCombineIntersectAndStoreAsync(string destination, params string[] keys)
        {
            return await _SetCombineAndStoreAsync(SetOperation.Intersect, destination, keys);
        }

        /// <summary>
        /// 获取几个集合的差集,并保存到一个新Key中
        /// </summary>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        public async Task<long> SetCombineDifferenceAndStoreAsync(string destination, params string[] keys)
        {
            return await _SetCombineAndStoreAsync(SetOperation.Difference, destination, keys);
        }

        #endregion 异步方法

        #region 内部辅助方法

        /// <summary>
        /// 获取几个集合的交叉并集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">Union：并集  Intersect：交集  Difference：差集  详见 <see cref="SetOperation"/></param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        private List<T> _SetCombine<T>(SetOperation operation, params string[] keys)
        {
            var rValue = base.DataBase.SetCombine(operation, ConvertToRedisKeys(keys));
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 获取几个集合的交叉并集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">Union：并集  Intersect：交集  Difference：差集  详见 <see cref="SetOperation"/></param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        private async Task<List<T>> _SetCombineAsync<T>(SetOperation operation, params string[] keys)
        {
            var rValue = await base.DataBase.SetCombineAsync(operation, ConvertToRedisKeys(keys));
            return ConvetList<T>(rValue);
        }

        /// <summary>
        /// 获取几个集合的交叉并集合,并保存到一个新Key中
        /// </summary>
        /// <param name="operation">Union：并集  Intersect：交集  Difference：差集  详见 <see cref="SetOperation"/></param>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        private long _SetCombineAndStore(SetOperation operation, string destination, params string[] keys)
        {
            return base.DataBase.SetCombineAndStore(operation, destination, ConvertToRedisKeys(keys));
        }

        /// <summary>
        /// 获取几个集合的交叉并集合,并保存到一个新Key中
        /// </summary>
        /// <param name="operation">Union：并集  Intersect：交集  Difference：差集  详见 <see cref="SetOperation"/></param>
        /// <param name="destination">保存的新Key名称</param>
        /// <param name="keys">要操作的Key集合</param>
        /// <returns></returns>
        private async Task<long> _SetCombineAndStoreAsync(SetOperation operation, string destination, params string[] keys)
        {
            return await base.DataBase.SetCombineAndStoreAsync(operation, destination, ConvertToRedisKeys(keys));
        }

        #endregion 内部辅助方法
    }
}