using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity.Base
{
    public class BaseEntity
    {
        public long Id { get; set; }

        public DateTime? CreateTime { get; set; }

        public long CreateUserId { get; set; }

        public DateTime? ModifyTime { get; set; }

        public long ModifyUserId { get; set; }

        public int IsDeleted { get; set; }

        public int Status { get; set; }

    }
}
