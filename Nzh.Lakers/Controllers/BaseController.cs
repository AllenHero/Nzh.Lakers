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
        /// <param name="Status"></param>
        /// <returns></returns>
        public static JsonResult Result(bool Status)
        {
            ResultModel result = new ResultModel();
            result.Status = Status;
            if (Status)
            {
                result.Message = "操作成功";
                result.Code = (int)StatusCodeType.Success;
            }
            else {
                result.Message = "操作成功";
                result.Code = (int)StatusCodeType.Error;
            } 
            return new JsonResult(result);
        }

        /// <summary>
        /// 返回封装
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static JsonResult Result(bool Status, string Message)
        {
            ResultModel result = new ResultModel();
            result.Status = Status;
            result.Message = Message;
            if (Status)
            {
                result.Code = (int)StatusCodeType.Success;
            }
            else {
                result.Code = (int)StatusCodeType.Error;
            }
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
            result.Status = true;
            result.Code = (int)StatusCodeType.Success;
            result.Message = StatusCodeType.Success.GetEnumText();
            result.Data = data;
            return new JsonResult(result);
        }

        #endregion 
    }
}
