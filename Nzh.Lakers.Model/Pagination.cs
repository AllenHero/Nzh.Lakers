﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Model
{
    /// <summary>
    /// 分页返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pagination<T>
    {
        /// <summary>
        ///  分页返回类
        /// </summary>
        public Pagination()
        {

        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 一页展示的条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalCount > 0)
                {
                    return TotalCount % this.PageSize == 0 ? TotalCount / this.PageSize : TotalCount / this.PageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
            set { }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public dynamic List { get; set; }
    }
}
