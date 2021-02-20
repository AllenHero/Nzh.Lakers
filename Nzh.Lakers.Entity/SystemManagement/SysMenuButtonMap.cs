using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_menubuttonmap")]
    public class SysMenuButtonMap
    {
        public long Id { get; set; }

        public long MenuId { get; set; }

        public long ButtonId { get; set; }
    }
}
