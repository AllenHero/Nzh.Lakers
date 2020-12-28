using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nzh.Lakers.Global
{
    public class GlobalExceptions : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;

        private readonly ILogger<GlobalExceptions> _logger;

        public GlobalExceptions(IHostingEnvironment env, ILogger<GlobalExceptions> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            //这里面是自定义的操作记录日志
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                if (_env.IsDevelopment())
                {
                    json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
                }
                context.Result = new BadRequestObjectResult(json);//返回异常数据
            }
            else
            {
                json.Message = "发生了未知内部错误";
                if (_env.IsDevelopment())
                {
                    json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
                }
                context.Result = new InternalServerErrorObjectResult(json);
            }
            //采用log4net 进行错误日志记录
            //LogHelper.ErrorLog(json.Message, context.Exception);
            _logger.LogError(json.Message, context.Exception);
        }
    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
