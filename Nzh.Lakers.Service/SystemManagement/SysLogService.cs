using Nzh.Lakers.Entity.SystemManagement;
using Nzh.Lakers.IRepository.SystemManagement;
using Nzh.Lakers.IService;
using Nzh.Lakers.IService.SystemManagement;
using Nzh.Lakers.Service.Base;
using Nzh.Lakers.Util.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Service.SystemManagement
{
    public class SysLogService : BaseService, ISysLogService
    {
        private ISysLogRepository _sysLogRepository;

        private readonly IUserHelper _userHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserRepository"></param>
        /// <param name="userHelper"></param>
        public SysLogService(ISysLogRepository sysLogRepository, IUserHelper userHelper)
        {
            _sysLogRepository = sysLogRepository;
            _userHelper = userHelper;
        }

        /// <summary>
        /// 新增日志
        /// </summary>
        /// <param name="sysLog"></param>
        /// <returns></returns>
        public bool InsertLog(SysLog sysLog)
        {
            try
            {
                _sysLogRepository.BeginTran();//开始事务
                SysLog Log = new SysLog();
                Log.Id = IdWorkerHelper.NewId();
                Log.LogStatus = sysLog.LogStatus;
                Log.IpAddress = sysLog.IpAddress;
                Log.LogType = sysLog.LogType;
                Log.Remark = sysLog.Remark;
                Log.CreateTime = DateTime.Now;
                Log.CreateUserId = _userHelper.Id;
                bool result = _sysLogRepository.Insert(Log);
                _sysLogRepository.CommitTran();
                return result;
            }
            catch (Exception ex)
            {
                _sysLogRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }
    }
}
