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

namespace Nzh.Lakers.Service
{
    public class DemoService : BaseService, IDemoService
    {
        private IDemoRepository _demoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="demoRepository"></param>
        public DemoService(IDemoRepository demoRepository)
        {
            _demoRepository = demoRepository;
        }

        /// <summary>
        /// 获取Demo分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public ResultModel<Demo> GetDemoPageList(int PageIndex, int PageSize, string Name)
        {
            PageModel pm = new PageModel() { PageIndex = PageIndex, PageSize = PageSize };
            var expression = ListFilter(Name);
            List<Demo> list = _demoRepository.GetPageList(expression, pm);
            ResultModel<Demo> rm = new ResultModel<Demo>();
            rm.Count = pm.PageCount;
            rm.Data = list;
            //int a = 0;
            //int b = 100 / a;
            return rm;
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
        /// 获取Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResultModel<Demo> GetDemoById(long Id)
        {
            ResultModel<Demo> rm = new ResultModel<Demo>();
            Demo model = _demoRepository.GetById(Id);
            rm.Data = model;
            return rm;
        }

        /// <summary>
        /// 新增Demo
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public ResultModel<bool> InsertDemo(string Name, string Sex, int Age, string Remark)
        {
            try
            {
                _demoRepository.BeginTran();//开始事务
                ResultModel<bool> rm = new ResultModel<bool>();
                Demo Demo = new Demo();
                Demo.Id = IdWorkerHelper.NewId();
                Demo.Name = Name;
                Demo.Sex = Sex;
                Demo.Age = Age;
                Demo.Remark = Remark;
                bool result = _demoRepository.Insert(Demo);
                _demoRepository.CommitTran();
                rm.Data = result;
                return rm;
            }
            catch (Exception ex)
            {
                _demoRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        /// <summary>
        /// 修改Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public ResultModel<bool> UpdateDemo(long Id, string Name, string Sex, int Age, string Remark)
        {
            try
            {
                _demoRepository.BeginTran();//开始事务
                ResultModel<bool> rm = new ResultModel<bool>();
                Demo Demo = new Demo();
                Demo.Id = Id;
                Demo.Name = Name;
                Demo.Sex = Sex;
                Demo.Age = Age;
                Demo.Remark = Remark;
                bool result = _demoRepository.Update(Demo);
                _demoRepository.CommitTran();
                rm.Data = result;
                return rm;
            }
            catch (Exception ex)
            {
                _demoRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        /// <summary>
        /// 删除Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResultModel<bool> DeleteDemoById(long Id)
        {
            try
            {
                _demoRepository.BeginTran();//开始事务
                ResultModel<bool> rm = new ResultModel<bool>();
                bool result = _demoRepository.DeleteById(Id);
                _demoRepository.CommitTran();
                rm.Data = result;
                return rm;
            }
            catch (Exception ex)
            {
                _demoRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }
    }
}
