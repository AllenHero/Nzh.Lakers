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
    [Route("api/Test")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="testService"></param>
        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        /// <summary>
        /// 获取Demo分页（异步）
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet("GetDemoPageListAsync")]
        public async Task<JsonResult> GetDemoPageListAsync(int PageIndex, int PageSize, string Name)
        {
            ResultModel<Demo> Result = new ResultModel<Demo>();
            try
            {
                Result = await _testService.GetDemoPageListAsync(PageIndex, PageSize, Name);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 获取Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetDemoByIdAsync")]
        public async Task<JsonResult> GetDemoByIdAsync(long Id)
        {
            ResultModel<Demo> Result = new ResultModel<Demo>();
            try
            {
                Result = await _testService.GetDemoByIdAsync(Id);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 添加Demo（异步）
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost("InsertDemoAsync")]
        public async Task<JsonResult> InsertDemoAsync(string Name, string Sex, int Age, string Remark)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                Result = await _testService.InsertDemoAsync(Name, Sex, Age, Remark);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 修改Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Sex"></param>
        /// <param name="Age"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost("UpdateDemoAsync")]
        public async Task<JsonResult> UpdateDemoAsync(long Id, string Name, string Sex, int Age, string Remark)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                Result = await _testService.UpdateDemoAsync(Id, Name, Sex, Age, Remark);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 删除Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("DeleteDemoByIdAsync")]
        public async Task<JsonResult> DeleteDemoByIdAsync(long Id)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                Result = await _testService.DeleteDemoByIdAsync(Id);
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
