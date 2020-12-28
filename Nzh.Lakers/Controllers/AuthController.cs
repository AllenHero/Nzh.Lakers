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
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<DemoController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public AuthController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            return Ok(new { Token = BuildToken("admin") });
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private string BuildToken(string UserId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("Security:Tokens:Key");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = "Security:Tokens:Issuer",
                    Audience = "Security:Tokens:Audience",
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, UserId) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(JsonConvert.SerializeObject(ex));
                return null;
            }
        }
    }
}
