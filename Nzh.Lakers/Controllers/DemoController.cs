using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nzh.Lakers.Entity;
using Nzh.Lakers.IService;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nzh.Lakers.Controllers
{
    [Route("api/[Demo]")]
    [ApiController]
    public class DemoController : Controller
    {
        private readonly IDemoService _demoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="demoService"></param>
        public DemoController(IDemoService demoService)
        {
            _demoService = demoService;
        }

        /// <summary>
        /// 获取Demo分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet("GetDemoPageList")]
        public JsonResult GetDemoPageList(int PageIndex, int PageSize, string Name)
        {
            ResultModel<Demo> Result = new ResultModel<Demo>();
            try
            {
                Result = _demoService.GetDemoPageList(PageIndex, PageSize, Name);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 获取Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetDemoById")]
        public JsonResult GetDemoById(long Id)
        {
            ResultModel<Demo> Result = new ResultModel<Demo>();
            try
            {
                Result = _demoService.GetDemoById(Id);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 添加Demo
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost("InsertDemo")]
        public JsonResult InsertDemo(string Name, string Sex, int Age, string Remark)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                Result = _demoService.InsertDemo(Name, Sex, Age, Remark);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 修改Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost("UpdateDemo")]
        public JsonResult UpdateDemo(long Id, string Name, string Sex, int Age, string Remark)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                Result = _demoService.UpdateDemo(Id, Name, Sex, Age, Remark);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 删除Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("DeleteDemoById")]
        public JsonResult DeleteDemoById(long Id)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                Result = _demoService.DeleteDemoById(Id);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }
    }
}
