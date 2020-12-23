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
        private ConnectionMultiplexer Redis { get; set; }

        private IDatabase DB { get; set; }

        public IConfiguration _Configuration { get; }

        public RedisCacheService(IConfiguration Configuration)
        {
            _Configuration = Configuration;
            string Connection = _Configuration.GetSection("Redis:ConnectionString").Value;
            Redis = ConnectionMultiplexer.Connect(Connection);
            DB = Redis.GetDatabase();
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
                return DB.StringSet(key,Convert.ToString(value));
            }
            return true;
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
                return DB.StringGet(key);
            }
            return null;
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
                return DB.KeyDelete(key);
            }
            return false;
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
            return !string.IsNullOrEmpty(DB.StringGet(key)) ? true : false;
        }
    }
}
