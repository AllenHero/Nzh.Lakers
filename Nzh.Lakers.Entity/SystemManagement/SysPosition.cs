using Nzh.Lakers.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_position")]
    public class SysPosition : BaseEntity
    {
        public string PositionName { get; set; }

        public long ParentId { get; set; }

        public string Remark { get; set; }
    }
}
