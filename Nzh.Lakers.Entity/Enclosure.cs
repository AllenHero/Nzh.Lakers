using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Entity
{
    [SugarTable("Enclosure")]
    public class Enclosure
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long Id { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [SugarColumn(ColumnName = "FilePath")]
        public string FilePath { get; set; }
    }
}
