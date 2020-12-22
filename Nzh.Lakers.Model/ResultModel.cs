using System;

namespace Nzh.Lakers.Model
{
    /// <summary>
    /// 表格数据，支持分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "成功";

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; set; }
    }
}
