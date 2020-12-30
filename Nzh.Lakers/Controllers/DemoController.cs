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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Nzh.Lakers.MQ.Helper;
using Nzh.Lakers.MQ;
using Nzh.Lakers.Util.Helper;

namespace Nzh.Lakers.Controllers
{
    [Route("api/Demo")]
    [ApiController]
    public class DemoController : BaseController
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
        /// <param name="logger"></param>
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetDemoPageList(int PageIndex, int PageSize, string Name)
        {
            var result = _demoService.GetDemoPageList(PageIndex, PageSize, Name);
            _memoryCache.Add("GetDemoPageList", JsonConvert.SerializeObject(result));
            //_redisCache.Add("GetDemoPageList", JsonConvert.SerializeObject(result));
            return Result(result);
        }

        /// <summary>
        /// 获取Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetDemoById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetDemoById(long Id)
        {
            var result = _demoService.GetDemoById(Id);
            return Result(result);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult InsertDemo(string Name, string Sex, int Age, string Remark)
        {
            var result = _demoService.InsertDemo(Name, Sex, Age, Remark);
            return Result(result);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult UpdateDemo(long Id, string Name, string Sex, int Age, string Remark)
        {
            var result = _demoService.UpdateDemo(Id, Name, Sex, Age, Remark);
            return Result(result);
        }

        /// <summary>
        /// 删除Demo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("DeleteDemoById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult DeleteDemoById(long Id)
        {
            var result = _demoService.DeleteDemoById(Id);
            return Result(result);
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpPost("GetCacheValue")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetCacheValue(string Key)
        {
            //var result = _redisCache.GetValue(Key);
            var result = _memoryCache.GetValue(Key);
            return Result(result);
        }

        /// <summary>
        /// 测试发送
        /// </summary>
        /// <param name="Msg"></param>
        /// <returns></returns>
        [HttpPost("TestSendMsg")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult TestSendMsg(string Msg)
        {
            //new RabbitMQHelper().Send("TestChange", "HelloRouteKey", "HelloMQ", Msg);
            var rabbitMqProxy = new RabbitMqService(new MqConfig
            {
                AutomaticRecoveryEnabled = true,
                HeartBeat = TimeSpan.FromTicks(60),
                NetworkRecoveryInterval = new TimeSpan(60),
                Host = "127.0.0.1",
                UserName = "Administrator",
                Password = "123456"
            });
            var log = new MessageModel
            {
                CreateDateTime = DateTime.Now,
                Msg = Msg
            };
            rabbitMqProxy.Publish(log);
            rabbitMqProxy.Dispose();
            return Result(true);
        }

        /// <summary>
        /// 测试接收
        /// </summary>
        /// <returns></returns>
        [HttpPost("TestReceiveMsg")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult TestReceiveMsg()
        {
            //var result = new RabbitMQHelper().Receive("TestChange", "HelloRouteKey", "HelloMQ");
            var json = "";
            var _rabbitMqProxy = new RabbitMqService(new MqConfig
            {
                AutomaticRecoveryEnabled = true,
                HeartBeat = TimeSpan.FromTicks(60),
                NetworkRecoveryInterval = new TimeSpan(60),
                Host = "127.0.0.1",
                UserName = "Administrator",
                Password = "123456"
            });
            _rabbitMqProxy.Subscribe<MessageModel>(msg =>
            {
                json = JsonConvert.SerializeObject(msg);
                Console.WriteLine(json);
            });
            _rabbitMqProxy.Dispose();
            return Result(json);
        }
    }
}
