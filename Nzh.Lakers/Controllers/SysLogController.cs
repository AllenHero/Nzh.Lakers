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
    [Route("api/SysLog")]
    [ApiController]
    public class SysLogController : BaseController
    {
        private readonly ILogger<SysLogController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public SysLogController(ILogger<SysLogController> logger)
        {
            _logger = logger;
        }
    }
}
