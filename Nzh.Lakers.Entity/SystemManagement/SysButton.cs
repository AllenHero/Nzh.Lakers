using Nzh.Lakers.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_button")]
    public class SysButton: BaseEntity
    {
        public string ButtonName { get; set; }

        public string ButtonIcon { get; set; }

        public string Remark { get; set; }

    }
}
