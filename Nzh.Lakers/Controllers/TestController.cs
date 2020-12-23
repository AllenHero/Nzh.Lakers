using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nzh.Lakers.Entity;
using Nzh.Lakers.IService;
using Nzh.Lakers.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nzh.Lakers.Controllers
{
    [Route("api/Test")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        private readonly IEnclosureService _enclosureService;

        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="testService"></param>
        /// <param name="enclosureService"></param>
        /// <param name="hostingEnvironment"></param>
        public TestController(ITestService testService, IEnclosureService enclosureService, IHostingEnvironment hostingEnvironment)
        {
            _testService = testService;
            _enclosureService = enclosureService;
            _hostingEnvironment = hostingEnvironment;
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

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="uploadfile"></param>
        /// <returns></returns>
        [HttpPost("TestUpLoadEnclosure")]
        public JsonResult TestUpLoadEnclosure(IFormFile uploadfile)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                uploadfile = Request.Form.Files[0];
                var now = DateTime.Now;
                //var webRootPath = _hostingEnvironment.ContentRootPath;
                var webRootPath = @"D:\Github\Nzh.Lakers\Nzh.Lakers";
                var filePath = string.Format(@"\UpLoadFile\{0}\", now.ToString("yyyyMMdd"));
                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                if (uploadfile != null)
                {
                    //文件后缀
                    var fileExtension = Path.GetExtension(uploadfile.FileName);
                    //判断后缀是否是图片
                    const string fileFilt = ".gif|.jpg|.php|.jsp|.jpeg|.png|......"; //图片
                    //const string fileFilt = ".doc|.xls|.ppt|.txt|.pdf|.html|......";   //附件
                    if (fileExtension == null)
                    {
                        return new JsonResult(new ResultModel<string> { Code = -1, Msg = "上传的文件没有后缀" });
                    }
                    if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        return new JsonResult(new ResultModel<string> { Code = -1, Msg = "上传的文件不是图片" });
                    }
                    //判断文件大小    
                    long length = uploadfile.Length;
                    if (length > 1024 * 1024 * 2) //2M
                    {
                        return new JsonResult(new ResultModel<string> { Code = -1, Msg = "上传的文件不能大于2M" });
                    }
                    var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                    var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                    var saveName = strDateTime + strRan + fileExtension;
                    //插入图片数据
                    string FilePath = filePath + saveName;
                    using (FileStream fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        uploadfile.CopyTo(fs);
                        fs.Flush();
                    }
                    Result = _enclosureService.TestUpLoadEnclosure(FilePath);
                    return new JsonResult(new ResultModel<string> { Code = 0, Msg = "上传成功", });
                }
                return new JsonResult(new ResultModel<string> { Code = -1, Msg = "上传失败" });
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Msg = ex.Message;
            }
            return Json(Result);
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("TestDownLoadEnclosure")]
        public JsonResult TestDownLoadEnclosure(long Id)
        {
            ResultModel<bool> Result = new ResultModel<bool>();
            try
            {
                //var webRootPath = _hostingEnvironment.ContentRootPath;
                var webRootPath = @"D:\Github\Nzh.Lakers\Nzh.Lakers";
                Enclosure Enclosure = _enclosureService.TestDownLoadEnclosure(Id);
                var addrUrl = Path.Combine(Directory.GetCurrentDirectory(), $@"{webRootPath + Enclosure.FilePath}");
                FileStream fs = new FileStream(addrUrl, FileMode.Open);
                var info = File(fs, "application/vnd.android.package-archive", Enclosure.FilePath);
                return new JsonResult(new ResultModel<string> { Code = 0, Msg = "下载成功", });
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
