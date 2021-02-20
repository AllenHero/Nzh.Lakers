using Nzh.Lakers.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_menu")]
    public class SysMenu: BaseEntity
    {
        public string MenuName { get; set; }

        public long ParentId { get; set; }

        public string MenuIcon { get; set; }

        public string MenuUrl { get; set; }

        public int MenuSort { get; set; }

        public string Remark { get; set; }
    }
}
