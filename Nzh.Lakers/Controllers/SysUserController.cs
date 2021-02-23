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
using Nzh.Lakers.IService.SystemManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Nzh.Lakers.Util.Security;
using Nzh.Lakers.Model.Dto;

namespace Nzh.Lakers.Controllers
{
    [Route("api/SysUser")]
    [ApiController]
    public class SysUserController : BaseController
    {
        private readonly ILogger<SysUserController> _logger;

        private readonly ISysUserService _sysUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysUserService"></param>
        public SysUserController(ILogger<SysUserController> logger, ISysUserService sysUserService)
        {
            _logger = logger;
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 用户列表分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Account"></param>
        /// <param name="RealName"></param>
        /// <param name="DepartmentId"></param>
        /// <param name="PositionId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpGet("GetUserPageList")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetUserPageList(int PageIndex, int PageSize, string Account, string RealName, long? DepartmentId,long? PositionId, int? Status)
        {
            var result = _sysUserService.GetUserPageList(PageIndex, PageSize, Account, RealName, DepartmentId, PositionId, Status);
            return Result(result);
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetUserById")]
        //Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetUserById(long Id)
        {
            var result = _sysUserService.GetUserById(Id);
            return Result(result);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("InsertUser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult InsertUser(UserDto userDto)
        {
            var result = _sysUserService.InsertUser(userDto);
            return Result(result);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        [HttpPost("TestEncrypt")]
        public JsonResult TestEncrypt(string Text)
        {
            var result = DESEncrypt.Encrypt(Text);
            return Result(result);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        [HttpPost("TestDecrypt")]
        public JsonResult TestDecrypt(string Text)
        {
            var result = DESEncrypt.Decrypt(Text);
            return Result(result);
        }
    }
}
