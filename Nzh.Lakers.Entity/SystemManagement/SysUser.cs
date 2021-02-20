using Nzh.Lakers.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_user")]
    public class SysUser : BaseEntity
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public string RealName { get; set; }

        public int Gender { get; set; }

        public long DepartmentId { get; set; }

        public long PositionId { get; set; }

        public string Birthday { get; set; }

        public string Portrait { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int IsSystem { get; set; }

        public int LoginCount { get; set; }

        public DateTime? FirstVisit { get; set; }

        public DateTime? LastVisit { get; set; }

        public string Remark { get; set; }

    }
}
