using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity
{
    public class Enclosure
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string FilePath { get; set; }
    }
}
