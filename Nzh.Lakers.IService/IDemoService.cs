using Nzh.Lakers.Entity;
using Nzh.Lakers.IService.Base;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace Nzh.Lakers.IService
{
    public interface IDemoService : IBaseService
    {
        Pagination<Demo> GetDemoPageList(int PageIndex, int PageSize, string Name);

        Demo GetDemoById(long Id);

        bool InsertDemo(string Name, string Sex, int Age, string Remark);

        bool UpdateDemo(long Id, string Name, string Sex, int Age, string Remark);

        bool DeleteDemoById(long Id);
    }
}
