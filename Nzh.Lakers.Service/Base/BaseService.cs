using Nzh.Lakers.Util.Extension;
using Nzh.Lakers.Util.Helper;
using Nzh.Lakers.Util.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Service.Base
{
    public class BaseService
    {
        public LoginUserDto UserCookie;

        public LoginUserDto GetUserCookie()
        {
            var userClaims = CookieHelper.GetUserLoginCookie();
            if (userClaims != null)
                return userClaims.ToObject<LoginUserDto>();
            return new LoginUserDto();
        }
    }
}
