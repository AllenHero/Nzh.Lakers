using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Util.Web
{
    public class LoginOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

    }
}
