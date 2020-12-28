using Nzh.Lakers.Entity;
using Nzh.Lakers.IRepository;
using Nzh.Lakers.IService;
using Nzh.Lakers.Model;
using Nzh.Lakers.Service.Base;
using Nzh.Lakers.Util.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Service
{
    public class EnclosureService:BaseService, IEnclosureService
    {
        private readonly IEnclosureRepository _enclosureRepository;

        public EnclosureService(IEnclosureRepository enclosureRepository)
        {
            _enclosureRepository = enclosureRepository;
        }

        /// <summary>
        /// 测试图片上传
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public bool TestUpLoadEnclosure(string FilePath)
        {
            try
            {
                _enclosureRepository.BeginTran();//开始事务
                Enclosure Enclosure = new Enclosure();
                Enclosure.Id= IdWorkerHelper.NewId();
                Enclosure.FilePath = FilePath;
                bool result = _enclosureRepository.Insert(Enclosure);
                _enclosureRepository.CommitTran();//提交事务
                return result;
            }
            catch (Exception ex)
            {
                _enclosureRepository.RollbackTran();//回滚事务
                throw ex;
            }
        }

        /// <summary>
        /// 测试图片下载
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Enclosure TestDownLoadEnclosure(long Id)
        {
            Enclosure model = _enclosureRepository.GetById(Id);
            return model;
        }
    }
}
