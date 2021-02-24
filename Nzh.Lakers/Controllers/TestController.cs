using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Nzh.Lakers.Entity;
using Nzh.Lakers.IService;
using Nzh.Lakers.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nzh.Lakers.Controllers
{
    [Route("api/Test")]
    [ApiController]
    public class TestController : BaseController
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> GetDemoPageListAsync(int PageIndex, int PageSize, string Name)
        {
            var result = await _testService.GetDemoPageListAsync(PageIndex, PageSize, Name);
            return Result(result);
        }

        /// <summary>
        /// 获取Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetDemoByIdAsync")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> GetDemoByIdAsync(long Id)
        {
            var result = await _testService.GetDemoByIdAsync(Id);
            return Result(result);
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> InsertDemoAsync(string Name, string Sex, int Age, string Remark)
        {
            var result = await _testService.InsertDemoAsync(Name, Sex, Age, Remark);
            return Result(result);
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> UpdateDemoAsync(long Id, string Name, string Sex, int Age, string Remark)
        {
            var result = await _testService.UpdateDemoAsync(Id, Name, Sex, Age, Remark);
            return Result(result);
        }

        /// <summary>
        /// 删除Demo（异步）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("DeleteDemoByIdAsync")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> DeleteDemoByIdAsync(long Id)
        {
            var result = await _testService.DeleteDemoByIdAsync(Id);
            return Result(result);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="uploadfile"></param>
        /// <returns></returns>
        [HttpPost("TestUpLoadEnclosure")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult TestUpLoadEnclosure(IFormFile uploadfile)
        {
            uploadfile = Request.Form.Files[0];
            var webRootPath = _hostingEnvironment.WebRootPath;
            var filePath = string.Format(@"\upload\{0}\", DateTime.Now.ToString("yyyyMMdd"));
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
                var result = _enclosureService.TestUpLoadEnclosure(FilePath);
                return Result(result);
            }
            return Result(false);
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="uploadfile"></param>
        /// <returns></returns>
        [HttpPost("TestImportExcel")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult TestImportExcel(IFormFile uploadfile)
        {
            var filename = ContentDispositionHeaderValue.Parse(uploadfile.ContentDisposition).FileName; // 原文件名（包括路径）
            var extName = filename.Substring(filename.LastIndexOf('.')).Replace("\"", "");// 扩展名
            string shortfilename = $"{Guid.NewGuid()}{extName}";// 新文件名
            string webRootPath = _hostingEnvironment.WebRootPath;//文件临时目录，导入完成后 删除
            var filePath = string.Format(@"\upload\{0}\", DateTime.Now.ToString("yyyyMMdd"));
            filename = webRootPath + filePath + shortfilename; // 新文件名（包括路径）
            if (!Directory.Exists(webRootPath + filePath))
            {
                Directory.CreateDirectory(webRootPath + filePath);
            }
            using (FileStream fs = System.IO.File.Create(filename)) // 创建新文件
            {
                uploadfile.CopyTo(fs);// 复制文件
                fs.Flush();// 清空缓冲区数据
            }
            FileInfo file = new FileInfo(filename);
            if (file != null)
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    var list = new List<Demo>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        Demo demo = new Demo();
                        demo.Id = Convert.ToInt64(worksheet.Cells[row, 1].Value.ToString());
                        demo.Name = worksheet.Cells[row, 2].Value.ToString();
                        demo.Sex = worksheet.Cells[row, 3].Value.ToString();
                        demo.Age = int.Parse(worksheet.Cells[row, 4].Value.ToString());
                        demo.Remark = worksheet.Cells[row, 5].Value.ToString();
                        list.Add(demo);
                    }
                   var result = _testService.TestImportExcel(list);
                   return Result(result);
                }
            }
            //处理完成后，删除上传的文件
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            return Result(true);
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("TestDownLoadEnclosure")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult TestDownLoadEnclosure(long Id)
        {
            var webRootPath = _hostingEnvironment.WebRootPath;
            Enclosure picture = new Enclosure();
            picture = _enclosureService.TestDownLoadEnclosure(Id);
            var addrUrl = Path.Combine(Directory.GetCurrentDirectory(), $@"{webRootPath + picture.FilePath}");
            FileStream fs = new FileStream(addrUrl, FileMode.Open);
            return File(fs, "application/vnd.android.package-archive", picture.FilePath);
        }

        /// <summary>
        /// 导出Execel
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet("TestExcelExport")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult TestExportExcel(string Name)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var filePath = string.Format(@"\download\{0}\", DateTime.Now.ToString("yyyyMMdd"));
            string sFileName = $@"ExcelExport{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            if (!Directory.Exists(webRootPath + filePath))
            {
                Directory.CreateDirectory(webRootPath + filePath);
            }
            var path = Path.Combine(webRootPath + filePath, sFileName);
            FileInfo file = new FileInfo(path);
            List<Demo> list = _testService.TestExportExcel(Name);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(path);
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("测试Test");
                worksheet.Cells.LoadFromCollection(list, true);
                package.Save();
            }
            return File(new FileStream(Path.Combine(webRootPath + filePath, sFileName), FileMode.Open), "application/octet-stream", $"导出Execel{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }
    }
}
