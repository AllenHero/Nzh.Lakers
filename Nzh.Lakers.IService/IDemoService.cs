using Nzh.Lakers.Entity;
using Nzh.Lakers.IService.Base;
using Nzh.Lakers.Model;
using System;

namespace Nzh.Lakers.IService
{
    public interface IDemoService : IBaseService
    {
        ResultModel<Demo> GetDemoPageList(int PageIndex, int PageSize, string Name);

        ResultModel<Demo> GetDemoById(long Id);

        ResultModel<bool> InsertDemo(string Name, string Sex, int Age, string Remark);

        ResultModel<bool> UpdateDemo(long Id, string Name, string Sex, int Age, string Remark);

        ResultModel<bool> DeleteDemoById(long Id);
    }
}
