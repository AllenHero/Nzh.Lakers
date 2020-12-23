using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nzh.Lakers.Cache.MemoryCache;
using Nzh.Lakers.Factory;
using Nzh.Lakers.SqlSugar;
using Nzh.Lakers.SwaggerHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nzh.Lakers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //注入服务
            services.AddRepositories();

            //读取数据库连接字符串
            BaseDBConfig.ConnectionString = this.Configuration.GetSection("Db:MySql").Value;

            //Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Nzh.Lakers WebAPI", Version = "v1" });
                // 为 Swagger 设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 添加控制器层注释，true表示显示控制器注释
                c.IncludeXmlComments(xmlPath, true);
                //添加对控制器的标签
                c.DocumentFilter<SwaggerDocTag>();
            });

            services.AddTransient<MemoryCacheService>(); //内存缓存认证注入 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nzh.Lakers API V1");
            });
        }
    }
}
