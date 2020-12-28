using Nzh.Lakers.Entity;
using Nzh.Lakers.IRepository;
using Nzh.Lakers.IService;
using Nzh.Lakers.Model;
using Nzh.Lakers.Service.Base;
using Nzh.Lakers.Util.Extension;
using Nzh.Lakers.Util.Helper;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Lakers.Service
{
    public class TestService : BaseService, ITestService
    {
        private ITestRepository _testRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="testRepository"></param>
        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        /// <summary>
        /// 获取Demo分（异步）
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public async Task<Pagination<Demo>> GetDemoPageListAsync(int PageIndex, int PageSize, string Name)
        {
            PageModel pm = new PageModel() { PageIndex = PageIndex, PageSize = PageSize };
            string sql = "SELECT * FROM  Demo";
            var expression = ListFilter(Name);
            Pagination<Demo> page = await _testRepository.GetPageListBySqlAsync(sql, expression, pm);
            return page;
        }

        /// <summary>
        /// 私有方法过滤查询条件
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private Expression<Func<Demo, bool>> ListFilter(string Name)
        {
            var expression = LinqExtensions.True<Demo>();
            if (!string.IsNullOrEmpty(Name))
            {
                expression = expression.And(t => t.Name.Contains(Name));
            }
            return expression;
        }

        /// <summary>
        ///  获取Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Demo> GetDemoByIdAsync(long Id)
        {
            string sql = "SELECT * FROM  Demo where Id=@Id";
            Demo model = await _testRepository.GetAsync(sql, new { Id = Id });
            return model;
        }

        /// <summary>
        /// 修改Demo（异步）
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public async Task<bool> InsertDemoAsync(string Name, string Sex, int Age, string Remark)
        {
            try
            {
                _testRepository.BeginTran();//开始事务
                var result = false;
                long Id = IdWorkerHelper.NewId();
                string sql = "INSERT INTO Demo(Id,Name,Sex,Age,Remark) VALUES(@Id,@Name,@Sex,@Age,@Remark)";
                SugarParameter[] Parameter = new SugarParameter[]
                {
               new SugarParameter("@Id",Id),
               new SugarParameter("@Name", Name),
               new SugarParameter("@Sex",  Sex),
               new SugarParameter("@Age", Age),
               new SugarParameter("@Remark", Remark)
               };
                result = await _testRepository.ExecuteSqlAsync(sql, Parameter);
                _testRepository.CommitTran();//提交事务
                return result;
            }
            catch (Exception ex)
            {
                _testRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        /// <summary>
        /// 修改Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDemoAsync(long Id, string Name, string Sex, int Age, string Remark)
        {
            try
            {
                _testRepository.BeginTran();//开始事务
                var result = false;
                string sql = "UPDATE Demo SET Name=@Name,Sex=@Sex,Age=@Age,Remark=@Remark WHERE Id=@Id";
                SugarParameter[] Parameter = new SugarParameter[]
                {
               new SugarParameter("@Id",Id),
               new SugarParameter("@Name", Name),
               new SugarParameter("@Sex",  Sex),
               new SugarParameter("@Age", Age),
               new SugarParameter("@Remark", Remark)
               };
                result = await _testRepository.ExecuteSqlAsync(sql, Parameter);
                _testRepository.CommitTran();//提交事务
                return result;
            }
            catch (Exception ex)
            {
                _testRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        /// <summary>
        /// 删除Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteDemoByIdAsync(long Id)
        {
            try
            {
                _testRepository.BeginTran();//开始事务
                var result = false;
                string sql = "DELETE FROM  Demo where Id=@Id";
                SugarParameter[] Parameter = new SugarParameter[]
                {
               new SugarParameter("@Id",Id)
                };
                result = await _testRepository.ExecuteSqlAsync(sql, Parameter);
                _testRepository.CommitTran();//提交事务
                return result;
            }
            catch (Exception ex)
            {
                _testRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool TestImportExcel(List<Demo> list)
        {
            try
            {
                _testRepository.BeginTran();//开始事务
                var result = false;
                foreach (var item in list)
                {
                    result = _testRepository.Insert(item);
                }
                _testRepository.CommitTran();//提交事务
                return result;
            }
            catch (Exception ex)
            {
                _testRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public List<Demo> TestExportExcel(string Name)
        {
            string sql = "SELECT * FROM  Demo";
            var expression = ListFilter(Name);
            List<Demo> list = _testRepository.GetListBySql(sql, expression);
            return list;
        }
    }
}
