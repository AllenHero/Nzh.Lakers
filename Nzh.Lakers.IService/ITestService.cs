using Nzh.Lakers.Entity;
using Nzh.Lakers.IService.Base;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Lakers.IService
{
    public interface ITestService : IBaseService
    {
        Task<ResultModel<Demo>> GetDemoPageListAsync(int PageIndex, int PageSize, string Name);

        Task<ResultModel<Demo>> GetDemoByIdAsync(long Id);

        Task<ResultModel<bool>> InsertDemoAsync(string Name, string Sex, int Age, string Remark);

        Task<ResultModel<bool>> UpdateDemoAsync(long Id, string Name, string Sex, int Age, string Remark);

        Task<ResultModel<bool>> DeleteDemoByIdAsync(long Id);

        ResultModel<bool> TestImportExcel(List<Demo> list);

        List<Demo> TestExportExcel(string Name);
    }
}
