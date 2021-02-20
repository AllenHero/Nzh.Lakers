using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.SystemManagement
{
    [SugarTable("sys_log")]
    public class SysLog
    {
        public long Id { get; set; }

        public int LogStatus { get; set; }

        public string IpAddress { get; set; }

        public string LogType { get; set; }

        public string Remark { get; set; }

        public DateTime? CreateTime { get; set; }

        public long CreateUserId { get; set; }
    }
}
