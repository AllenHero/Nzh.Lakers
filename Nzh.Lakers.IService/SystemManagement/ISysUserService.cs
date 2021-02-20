using Nzh.Lakers.Entity.SystemManagement;
using Nzh.Lakers.IService.Base;
using Nzh.Lakers.Model;
using Nzh.Lakers.Model.Param;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.IService.SystemManagement
{
    public interface ISysUserService : IBaseService
    {
        Pagination<SysUser> GetUserPageList(int PageIndex, int PageSize, string Account, string RealName, long? DepartmentId, long? PositionId, int? Status);

        SysUser GetUserById(long Id);

        bool InsertUser(UserParam Param);
    }
}
