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

namespace Nzh.Lakers.Controllers
{
    [Route("api/SysUser")]
    [ApiController]
    public class SysUserController : BaseController
    {
        private readonly ILogger<SysUserController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public SysUserController(ILogger<SysUserController> logger)
        {
            _logger = logger;
        }
    }
}
