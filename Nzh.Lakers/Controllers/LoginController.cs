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
using Nzh.Lakers.IService;
using Nzh.Lakers.Entity.SystemManagement;
using static Nzh.Lakers.Model.EnumType;

namespace Nzh.Lakers.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        private readonly ISysUserService _sysUserService;

        private readonly IUserToken _userToken;

        private readonly ISysLogService _sysLogService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysUserService"></param>
        /// <param name="userToken"></param>
        /// <param name="sysLogService"></param>
        public LoginController(ILogger<LoginController> logger, ISysUserService sysUserService, IUserToken userToken, ISysLogService sysLogService)
        {
            _logger = logger;
            _sysUserService = sysUserService;
            _userToken = userToken;
            _sysLogService = sysLogService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public JsonResult Login(LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Account))
            {
                return Result(false, "用户名不能为空");
            }
            if (string.IsNullOrEmpty(loginDto.Password))
            {
                return Result(false, "密码不能为空");
            }
            var output = new LoginOutput();
            SysLog sysLog = new SysLog();
            try
            {
                var user = _sysUserService.LoginValidate(loginDto.Account.Trim(), loginDto.Password.Trim());
                if (user != null)
                {
                    output.Id = user.Id;
                    output.Account = user.Account;
                    output.RealName = user.RealName;

                    #region 更新用户登录信息

                    user.Id = user.Id;
                    user.LoginCount++;
                    if (user.FirstVisit == null)
                    {
                        user.FirstVisit = DateTime.Now;
                    }
                    user.LastVisit = DateTime.Now;
                    _sysUserService.UpdateUserLoginInfo(user);

                    #endregion

                    sysLog.LogStatus = (int)LogStatusType.Success;
                }
                else
                {
                    sysLog.LogStatus = (int)LogStatusType.Fail;
                    return Result(false, "用户名或密码错误");
                }

                #region 登录日志

                sysLog.IpAddress = "";
                sysLog.LogType = LogTypeType.Login.ToString();
                sysLog.Remark = "";
                _sysLogService.InsertLog(sysLog);

                #endregion
            }
            catch (Exception ex)
            {
                return Result(false, "登录异常" + ex);
            }
            return GetToken(output);
        }

        private JsonResult GetToken(LoginOutput output)
        {
            var token = _userToken.Create(new[]
            {
                new Claim(LoginUserDto.Id.ToString(), output.Id.ToString()),
                new Claim(LoginUserDto.Account, output.Account),
                new Claim(LoginUserDto.RealName, output.RealName),
            });
            return Result(new { token });
        }
    }
}
