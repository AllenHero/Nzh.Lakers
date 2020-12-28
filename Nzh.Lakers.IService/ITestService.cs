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
        Task<Pagination<Demo>> GetDemoPageListAsync(int PageIndex, int PageSize, string Name);

        Task<Demo> GetDemoByIdAsync(long Id);

        Task<bool> InsertDemoAsync(string Name, string Sex, int Age, string Remark);

        Task<bool> UpdateDemoAsync(long Id, string Name, string Sex, int Age, string Remark);

        Task<bool> DeleteDemoByIdAsync(long Id);

        bool TestImportExcel(List<Demo> list);

        List<Demo> TestExportExcel(string Name);
    }
}
