using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.IService
{
    public interface IUserHelper
    {
        /// <summary>
        /// 主键
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string Account { get; }

        /// <summary>
        /// 昵称
        /// </summary>
        string RealName { get; }
    }
}
