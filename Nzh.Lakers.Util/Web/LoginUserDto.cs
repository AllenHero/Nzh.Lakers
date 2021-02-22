using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Util.Web
{
    public  class LoginUserDto
    {
        public long Id { get; set; }

        public string Account { get; set; }

        public string RealName { get; set; }

        public long DepartmentId { get; set; }

        public long PositionId { get; set; }
    }
}
