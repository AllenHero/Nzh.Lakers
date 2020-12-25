using SqlSugar;
using System;

namespace Nzh.Lakers.Entity
{
    /// <summary>
    /// 示例
    /// </summary>
    [SugarTable("Demo")]//当和数据库名称不一样可以设置别名
    public class Demo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]//通过特性设置主键和自增列 
        public long Id { get; set; }

        /// <summary>
        ///名称 
        /// </summary>
        [SugarColumn(ColumnName = "Name")]//数据库列名取自定义
        public string Name { get; set; }

        /// <summary>
        ///性别
        /// </summary>
        [SugarColumn(ColumnName = "Sex")]
        public string Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [SugarColumn(ColumnName = "Age")]
        public int Age { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }
    }
}
