﻿using Nzh.Lakers.Entity.SystemManagement;
using Nzh.Lakers.IService.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.IService.SystemManagement
{
    public interface ISysLogService : IBaseService
    {
        bool InsertLog(SysLog sysLog);
    }
}
