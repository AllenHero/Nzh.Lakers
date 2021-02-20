using Nzh.Lakers.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_dict")]
    public class SysDict : BaseEntity
    {
        public string DictType { get; set; }

        public long ParentId { get; set; }

        public string DictKey { get; set; }

        public string DictValue { get; set; }

        public string Remark { get; set; }
    }
}
