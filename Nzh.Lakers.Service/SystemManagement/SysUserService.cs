using Nzh.Lakers.Entity.SystemManagement;
using Nzh.Lakers.IRepository.SystemManagement;
using Nzh.Lakers.IService;
using Nzh.Lakers.IService.SystemManagement;
using Nzh.Lakers.Model;
using Nzh.Lakers.Model.Dto;
using Nzh.Lakers.Service.Base;
using Nzh.Lakers.Util.Extension;
using Nzh.Lakers.Util.Helper;
using Nzh.Lakers.Util.Security;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static Nzh.Lakers.Model.EnumType;

namespace Nzh.Lakers.Service.SystemManagement
{
    public class SysUserService : BaseService, ISysUserService
    {
        private ISysUserRepository _sysUserRepository;

        private readonly IUserHelper _userHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserRepository"></param>
        /// <param name="userHelper"></param>
        public SysUserService(ISysUserRepository sysUserRepository, IUserHelper userHelper)
        {
            _sysUserRepository = sysUserRepository;
            _userHelper = userHelper;
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
        /// <param name="userDto"></param>
        /// <returns></returns>
        public bool InsertUser(UserDto userDto)
        {
            try
            {
                _sysUserRepository.BeginTran();//开始事务
                SysUser User = new SysUser();
                User.Id = IdWorkerHelper.NewId();
                User.Account = userDto.Account;
                User.Password = DESEncrypt.Encrypt(userDto.Password);
                User.RealName = userDto.RealName;
                User.Gender = userDto.Gender;
                User.DepartmentId = userDto.DepartmentId;
                User.Birthday = userDto.Birthday;
                User.Portrait = userDto.Portrait;
                User.Email = userDto.Email;
                User.Phone = userDto.Phone;
                User.Remark = userDto.Remark;
                User.Status = (int)StatusType.Enabled;
                User.IsDeleted = (int)IsDeletedType.No;
                User.CreateTime = DateTime.Now;
                User.CreateUserId = _userHelper.Id;
                User.ModifyTime = DateTime.Now;
                User.ModifyUserId = _userHelper.Id;
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

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public SysUser LoginValidate(string Account, string Password)
        {
            Password = DESEncrypt.Encrypt(Password);
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

        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        public bool UpdateUserLoginInfo(SysUser sysUser)
        {
            try
            {
                _sysUserRepository.BeginTran();//开始事务
                string sql = "UPDATE sys_user SET LoginCount=@LoginCount,FirstVisit=@FirstVisit,LastVisit=@LastVisit WHERE Id=@Id";
                SugarParameter[] Parameter = new SugarParameter[]
                {
               new SugarParameter("@Id",sysUser.Id),
               new SugarParameter("@LoginCount", sysUser.LoginCount),
               new SugarParameter("@FirstVisit",  sysUser.FirstVisit),
               new SugarParameter("@LastVisit", sysUser.LastVisit)
               };
                bool result =  _sysUserRepository.ExecuteSql(sql, Parameter);
                _sysUserRepository.CommitTran();//提交事务
                return result;
            }
            catch (Exception ex)
            {
                _sysUserRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }
    }
}
