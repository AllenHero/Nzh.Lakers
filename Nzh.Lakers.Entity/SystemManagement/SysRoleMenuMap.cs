using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_rolemenumap")]
    public class SysRoleMenuMap
    {
        public long Id { get; set; }

        public long RoleId { get; set; }

        public long MenuId { get; set; }
    }
}
