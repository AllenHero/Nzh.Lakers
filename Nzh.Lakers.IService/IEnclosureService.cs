using Nzh.Lakers.Entity;
using Nzh.Lakers.IService.Base;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.IService
{
    public interface IEnclosureService : IBaseService
    {
        ResultModel<bool> TestUpLoadEnclosure(string FilePath);

        Enclosure TestDownLoadEnclosure(long Id);
    }
}
