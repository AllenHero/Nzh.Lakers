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
using Nzh.Lakers.Cache.RedisCache;
using Microsoft.Extensions.Logging;

namespace Nzh.Lakers.Controllers
{
    [Route("api/Demo")]
    [ApiController]
    public class DemoController : Controller
    {
        private readonly IDemoService _demoService;

        private readonly ICacheService _memoryCache;

        private readonly ICacheService _redisCache;

        private readonly ILogger<DemoController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="demoService"></param>
        /// <param name="memoryCache"></param>
        /// <param name="redisCache"></param>
        public DemoController(IDemoService demoService, ICacheService memoryCache, ICacheService redisCache, ILogger<DemoController> logger)
        {
            _demoService = demoService;
            _memoryCache = memoryCache;
            _redisCache= redisCache;
            _logger = logger;
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
            ResultModel<Demo> result = new ResultModel<Demo>();
            try
            {
                result = _demoService.GetDemoPageList(PageIndex, PageSize, Name);
                _memoryCache.Add("GetDemoPageList", JsonConvert.SerializeObject(result));
                _redisCache.Add("GetDemoPageList", JsonConvert.SerializeObject(result));
                _logger.LogInformation(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 获取Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetDemoById")]
        public JsonResult GetDemoById(long Id)
        {
            ResultModel<Demo> result = new ResultModel<Demo>();
            try
            {
                result = _demoService.GetDemoById(Id);
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = ex.Message;
            }
            return Json(result);
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
            ResultModel<bool> result = new ResultModel<bool>();
            try
            {
                result = _demoService.InsertDemo(Name, Sex, Age, Remark);
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = ex.Message;
            }
            return Json(result);
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
            ResultModel<bool> result = new ResultModel<bool>();
            try
            {
                result = _demoService.UpdateDemo(Id, Name, Sex, Age, Remark);
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 删除Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("DeleteDemoById")]
        public JsonResult DeleteDemoById(long Id)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            try
            {
                result = _demoService.DeleteDemoById(Id);
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpPost("GetCacheValue")]
        public JsonResult GetCacheValue(string Key)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                //result.Data = _memoryCache.GetValue(Key);
                result.Data = _redisCache.GetValue(Key);
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = ex.Message;
            }
            return Json(result);
        }
    }
}
