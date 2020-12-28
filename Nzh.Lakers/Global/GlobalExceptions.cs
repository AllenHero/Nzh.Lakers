using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Nzh.Lakers.Model.EnumType;

namespace Nzh.Lakers.Global
{
    public class GlobalExceptions : Attribute, IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        private readonly ILogger<GlobalExceptions> _logger;

        public GlobalExceptions(IWebHostEnvironment env, ILogger<GlobalExceptions> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            ResultModel<string> result = new ResultModel<string>();
            result.Code = (int)StatusCodeType.Error;
            result.Msg = context.Exception.Message;//错误信息
            if (_env.IsDevelopment())
            {
                result.Data = context.Exception.StackTrace;//堆栈信息
            }
            context.Result = new JsonResult(result) { StatusCode = (int)StatusCodeType.Success };
            //采用Nlog 进行错误日志记录
            _logger.LogError(WriteLog(result.Msg, context.Exception));
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return $"\r\n【自定义错误】：{throwMsg} \r\n【异常类型】：{ex.GetType().Name} \r\n【异常信息】：{ex.Message} \r\n【堆栈调用】：{ex.StackTrace }\r\n";
        }
    }
}
