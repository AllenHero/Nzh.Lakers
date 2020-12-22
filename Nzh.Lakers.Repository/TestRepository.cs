using Nzh.Lakers.Entity;
using Nzh.Lakers.IRepository;
using Nzh.Lakers.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Repository
{
    public class TestRepository : BaseRepository<Demo>, ITestRepository
    {

    }
}
