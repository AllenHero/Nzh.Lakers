using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Nzh.Lakers.Model.EnumType;

namespace Nzh.Lakers.Global
{
    public class GlobalActionMonitor : Attribute, IActionFilter
    {
        public GlobalActionMonitor()
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            #region 模型验证
            if (context.ModelState.IsValid) return;
            ResultModel result = new ResultModel();
            result.Code = (int)StatusCodeType.ParameterError;
            foreach (var item in context.ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    if (!string.IsNullOrEmpty(result.Msg))
                    {
                        result.Msg += " | ";
                    }
                    result.Msg += error.ErrorMessage;
                }
            }
            context.Result = new JsonResult(result);
            #endregion
        }
    }
}
