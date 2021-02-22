using Microsoft.AspNetCore.Http;
using Nzh.Lakers.IService;
using Nzh.Lakers.Util.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Service
{
    public class UserHelper: IUserHelper
    {
        private readonly IHttpContextAccessor _accessor;

        public UserHelper(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long Id
        {
            get
            {
                var id = _accessor?.HttpContext?.User?.FindFirst(LoginUserDto.Id);
                if (id != null && id.Value!="")
                {
                    return Convert.ToInt64(id.Value);
                }
                return 0;
            }
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account
        {
            get
            {
                var account = _accessor?.HttpContext?.User?.FindFirst(LoginUserDto.Account);
                if (account != null && account.Value!="")
                {
                    return account.Value;
                }
                return "";
            }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string RealName
        {
            get
            {
                var realname = _accessor?.HttpContext?.User?.FindFirst(LoginUserDto.RealName);
                if (realname != null && realname.Value!="")
                {
                    return realname.Value;
                }
                return "";
            }
        }
    }
}
