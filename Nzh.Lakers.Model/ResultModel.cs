using System;

namespace Nzh.Lakers.Model
{
    /// <summary>
    /// 统一接口返回
    /// </summary>
    public class ResultModel
    {
        public ResultModel()
        {
            Code = 0;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }

    public class ResultModel<T> : ResultModel
    {
        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; set; }
    }
}
