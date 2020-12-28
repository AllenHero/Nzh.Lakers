using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nzh.Lakers.SwaggerHelper
{
    /// <summary>
    /// Swagger注释帮助类
    /// </summary>
    public class SwaggerDocTag : IDocumentFilter
    {
        /// <summary>
        /// 添加附加注释
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                //添加对应的控制器描述 这个是我好不容易在issues里面翻到的
                new OpenApiTag { Name = "Auth", Description = "JWT认证" },
                new OpenApiTag { Name = "Demo", Description = "Demo示例" },
                new OpenApiTag { Name = "Test", Description = "Sql操作" },
            };
        }
    }
}
