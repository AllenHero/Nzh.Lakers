using Nzh.Lakers.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_role")]
    public class SysRole : BaseEntity
    {
        public string RoleName { get; set; }

        public string Remark { get; set; }
    }
}
