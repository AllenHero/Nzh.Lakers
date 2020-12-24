using Nzh.Lakers.Entity;
using Nzh.Lakers.IRepository;
using Nzh.Lakers.Repository.Base;
using System;

namespace Nzh.Lakers.Repository
{
    public class DemoRepository : BaseRepository<Demo>, IDemoRepository
    {
    }
}
