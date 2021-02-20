using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_userrolemap")]
    public class SysUserRoleMap
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long RoleId { get; set; }
    }
}
