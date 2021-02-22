using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Nzh.Lakers.Model.Dto;
using Nzh.Lakers.Util.Helper;
using Nzh.Lakers.Util.Web;
using Nzh.Lakers.IService.SystemManagement;
using Nzh.Lakers.Util.Extension;

namespace Nzh.Lakers.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        private readonly ISysUserService _sysUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysUserService"></param>
        public LoginController(ILogger<LoginController> logger, ISysUserService sysUserService)
        {
            _logger = logger;
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public JsonResult Login(LoginDto loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Account))
            {
                ModelState.AddModelError("err", "用户名不能为空");
            }
            if (string.IsNullOrEmpty(loginModel.Password))
            {
                ModelState.AddModelError("err", "密码不能为空");
            }
            try
            {
                var user = _sysUserService.LoginValidate(loginModel.Account.Trim(), loginModel.Password.Trim());
                var loginUserDto = new LoginUserDto();
                if (user != null)
                {
                    loginUserDto.Id = user.Id;
                    loginUserDto.Account = user.Account;
                    loginUserDto.RealName = user.RealName;
                    loginUserDto.DepartmentId = user.DepartmentId;
                    loginUserDto.PositionId = user.PositionId;
                    string claimstr = loginUserDto.ToJson();
                    CookieHelper.WriteLoginCookie(claimstr);
                }
                ModelState.AddModelError("err", "用户名或密码错误");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("err", "登录异常"+ ex);
            }
            return Result(true);
        }
    }
}
