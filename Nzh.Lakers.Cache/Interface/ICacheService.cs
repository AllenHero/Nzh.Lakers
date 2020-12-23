using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Cache.Interface
{
    /// <summary>
    /// 缓存接口
    /// 分别内存缓存和Redis缓存(2.1.0版本)
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        ///  新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ExpirtionTime"></param>
        /// <returns></returns>
        bool Add(string key, object value, int ExpirtionTime = 20);


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValue(string key);

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);
    }
}
