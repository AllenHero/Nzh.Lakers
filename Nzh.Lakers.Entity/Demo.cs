using System;

namespace Nzh.Lakers.Entity
{
    /// <summary>
    /// 示例
    /// </summary>
    public class Demo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
