using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nzh.Lakers.Cache.Interface;
using Nzh.Lakers.Cache.MemoryCache;
using Nzh.Lakers.Entity;
using Nzh.Lakers.IService;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Nzh.Lakers.Controllers
{
    [Route("api/Demo")]
    [ApiController]
    public class DemoController : Controller
    {
        private readonly IDemoService _demoService;

        private readonly ICacheService _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="demoService"></param>
        /// <param name="cacheService"></param>
        public DemoController(IDemoService demoService, IServiceProvider cacheService)
        {
            _demoService = demoService;
            _memoryCache = cacheService.GetService<MemoryCacheService>();
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
                _memoryCache.Add("GetDemoPageList", JsonConvert.SerializeObject(Result));
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

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpPost("GetCacheValue")]
        public JsonResult GetCacheValue(string Key)
        {
            ResultModel<string> Result = new ResultModel<string>();
            try
            {
                Result.Data = _memoryCache.GetValue(Key);
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
