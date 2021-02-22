using Google.Protobuf.WellKnownTypes;
using Microsoft.IdentityModel.Tokens;
using Nzh.Lakers.IService;
using Nzh.Lakers.Util.Web;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Nzh.Lakers.Service
{
    public class UserToken : IUserToken
    {
        private readonly JwtConfig _jwtConfig;

        public UserToken(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public string Create(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var timestamp = DateTime.Now.AddMinutes(3600).ToTimestamp().ToString();
            var timestamp = UnixTimeStampUTC(DateTime.Now).ToString();
            claims = claims.Append(new Claim(LoginUserDto.RefreshExpires, timestamp)).ToArray();

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtConfig.Expires),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static long UnixTimeStampUTC(DateTime dateTime)
        {
            Int32 unixTimeStamp;
            DateTime zuluTime = dateTime.ToUniversalTime();
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;
            return unixTimeStamp;
        }

        public Claim[] Decode(string jwtToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
            return jwtSecurityToken?.Claims?.ToArray();
        }
    }
}
