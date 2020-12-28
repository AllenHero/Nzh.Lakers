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
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }

    public class ResultModel<T> : ResultModel
    {
       
        /// <summary>
        /// 数量
        /// </summary>
        //public int Count { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; set; }
    }
}
