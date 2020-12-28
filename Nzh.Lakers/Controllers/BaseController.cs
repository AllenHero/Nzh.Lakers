using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Nzh.Lakers.Model.EnumType;

namespace Nzh.Lakers.Controllers
{
    public class BaseController : ControllerBase
    {
        #region 统一返回封装

        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static JsonResult Result(StatusCodeType statusCode)
        {
            ResultModel result = new ResultModel();
            result.Code = (int)statusCode;
            result.Msg = statusCode.GetEnumText();
            return new JsonResult(result);
        }

        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static JsonResult Result(StatusCodeType statusCode, string retMessage)
        {
            ResultModel result = new ResultModel();
            result.Code = (int)statusCode;
            result.Msg = retMessage;
            return new JsonResult(result);
        }

        /// <summary>
        /// 返回封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult Result<T>(T data)
        {
            ResultModel<T> result = new ResultModel<T>();
            result.Code = (int)StatusCodeType.Success;
            result.Msg = StatusCodeType.Success.GetEnumText();
            result.Data = data;
            return new JsonResult(result);
        }

        #endregion
    }
}
