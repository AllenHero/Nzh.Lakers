using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Nzh.Lakers.IService
{
    public interface IUserToken
    {
        string Create(Claim[] claims);

        Claim[] Decode(string jwtToken);
    }
}
