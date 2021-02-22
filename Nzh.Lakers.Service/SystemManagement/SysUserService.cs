using Nzh.Lakers.Entity.SystemManagement;
using Nzh.Lakers.IRepository.SystemManagement;
using Nzh.Lakers.IService.SystemManagement;
using Nzh.Lakers.Model;
using Nzh.Lakers.Model.Param;
using Nzh.Lakers.Service.Base;
using Nzh.Lakers.Util.Extension;
using Nzh.Lakers.Util.Helper;
using Nzh.Lakers.Util.Security;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Nzh.Lakers.Service.SystemManagement
{
    public class SysUserService : BaseService, ISysUserService
    {
        private ISysUserRepository _sysUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="testRepository"></param>
        public SysUserService(ISysUserRepository sysUserRepository)
        {
            _sysUserRepository = sysUserRepository;
        }

        /// <summary>
        /// 用户列表分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Account"></param>
        /// <param name="RealName"></param>
        /// <param name="DepartmentId"></param>
        /// <param name="PositionId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public Pagination<SysUser> GetUserPageList(int PageIndex, int PageSize, string Account, string RealName, long? DepartmentId, long? PositionId, int? Status)
        {
            PageModel pm = new PageModel() { PageIndex = PageIndex, PageSize = PageSize };
            var expression = ListFilter(Account,RealName, DepartmentId, PositionId, Status);
            Pagination<SysUser> page = _sysUserRepository.GetPageList(expression, pm);
            return page;
        }

        /// <summary>
        /// 私有方法过滤查询条件
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="RealName"></param>
        /// <param name="DepartmentId"></param>
        /// <param name="PositionId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        private Expression<Func<SysUser, bool>> ListFilter(string Account, string RealName, long? DepartmentId, long? PositionId, int? Status)
        {
            var expression = LinqExtensions.True<SysUser>();
            if (!string.IsNullOrEmpty(Account))
            {
                expression = expression.And(t => t.Account == Account);
            }
            if (!string.IsNullOrEmpty(RealName))
            {
                expression = expression.And(t => t.RealName.Contains(RealName));
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DepartmentId)))
            {
                expression = expression.And(t => t.DepartmentId == DepartmentId);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PositionId)))
            {
                expression = expression.And(t => t.PositionId == PositionId);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                expression = expression.And(t => t.Status == Status);
            }
            return expression;
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SysUser GetUserById(long Id)
        {
            SysUser model = _sysUserRepository.GetById(Id);
            return model;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public bool InsertUser(UserParam Param)
        {
            try
            {
                _sysUserRepository.BeginTran();//开始事务
                SysUser User = new SysUser();
                User.Id = IdWorkerHelper.NewId();
                User.Account = Param.Account;
                User.Password = DESEncrypt.Encrypt(Param.Password);
                User.RealName = Param.RealName;
                User.Gender = Param.Gender;
                User.DepartmentId = Param.DepartmentId;
                User.Birthday = Param.Birthday;
                User.Portrait = Param.Portrait;
                User.Email = Param.Email;
                User.Phone = Param.Phone;
                User.Remark = Param.Remark;
                User.Status = 0;
                User.IsDeleted = 0;
                User.CreateTime = DateTime.Now;
                User.CreateUserId = UserCookie.Id;
                User.ModifyTime = DateTime.Now;
                User.ModifyUserId = UserCookie.Id;
                bool result = _sysUserRepository.Insert(User);
                _sysUserRepository.CommitTran();
                return result;
            }
            catch (Exception ex)
            {
                _sysUserRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        public SysUser LoginValidate(string Account, string Password)
        {
            Password = DESEncrypt.Decrypt(Password);
            var expression = LinqExtensions.True<SysUser>();
            if (!string.IsNullOrEmpty(Account))
            {
                expression = expression.And(t => t.Account == Account);
            }
            if (!string.IsNullOrEmpty(Account))
            {
                expression = expression.And(t => t.Password == Password);
            }
            var model = _sysUserRepository.GetSingle(expression);
            return model;
        }
    }
}
