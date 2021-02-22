using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Util.Web
{
    public class LoginUserDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// 认证授权用户Id
        /// </summary>
        public const string Account = "account";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string RealName = "realname";


        /// <summary>
        /// 刷新有效期
        /// </summary>
        public const string RefreshExpires = "re";

    }
}
