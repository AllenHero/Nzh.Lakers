using Nzh.Lakers.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_department")]
    public class SysDepartment : BaseEntity
    {
        public string DepartmentName { get; set; }

        public long ParentId { get; set; }

        public string Remark { get; set; }
    }
}
