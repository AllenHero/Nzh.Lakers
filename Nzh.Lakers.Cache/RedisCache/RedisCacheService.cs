using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nzh.Lakers.Cache.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Cache.RedisCache
{
    public class RedisCacheService : ICacheService
    {
        private ConnectionMultiplexer _redis { get; set; }

        private IDatabase _cache { get; set; }

        public IConfiguration _configuration { get; }

        public RedisCacheService(IConfiguration configuration)
        {
            _configuration = configuration;
            string Connection = _configuration.GetSection("Redis:ConnectionString").Value;
            _redis = ConnectionMultiplexer.Connect(Connection);
            _cache = _redis.GetDatabase();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return _cache.StringSet(key,Convert.ToString(value));
            }
            return true;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (Exists(key))
            {
                return _cache.KeyDelete(key);
            }
            return false;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (Exists(key))
            {
                return _cache.StringGet(key);
            }
            return null;
        }

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return !string.IsNullOrEmpty(_cache.StringGet(key)) ? true : false;
        }
    }
}
