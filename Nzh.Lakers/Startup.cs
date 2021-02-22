using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Nzh.Lakers.Cache.MemoryCache;
using Nzh.Lakers.Factory;
using Nzh.Lakers.Global;
using Nzh.Lakers.IService;
using Nzh.Lakers.Job.HostedService;
using Nzh.Lakers.Job.Job;
using Nzh.Lakers.Job.JobFactory;
using Nzh.Lakers.Job.JobSchedule;
using Nzh.Lakers.Middleware;
using Nzh.Lakers.Service;
using Nzh.Lakers.SqlSugar;
using Nzh.Lakers.SwaggerHelper;
using Nzh.Lakers.Util.Helper;
using Nzh.Lakers.Util.Web;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Lakers
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        private readonly IHostEnvironment _env;

        private readonly ConfigHelper _configHelper;

        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _configHelper = new ConfigHelper();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //�û���Ϣ
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.TryAddSingleton<IUserHelper, UserHelper>();

            services.TryAddSingleton<IUserToken, UserToken>();

            var jwtConfig = _configHelper.Get<JwtConfig>("jwtconfig", _env.EnvironmentName);
            services.TryAddSingleton(jwtConfig);

            //ע�����
            services.AddRepositories();

            //��ȡ���ݿ������ַ���
            BaseDBConfig.ConnectionString = this._configuration.GetSection("Db:MySql").Value;

            //Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Nzh.Lakers WebAPI", Version = "v1" });
                // Ϊ Swagger ����xml�ĵ�ע��·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                c.IncludeXmlComments(xmlPath, true);
                //��ӶԿ������ı�ǩ
                c.DocumentFilter<SwaggerDocTag>();

                //JWT
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                     { securityScheme, new string[] { } }
                });

                services.AddControllers(options =>
                {
                    //ȫ���쳣����
                    options.Filters.Add<GlobalExceptions>();
                    //ȫ����־
                    options.Filters.Add<GlobalActionMonitor>();
                });
            });

            //Add Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "Security:Tokens:Issuer",
                        ValidateAudience = true,
                        ValidAudience = "Security:Tokens:Audience",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Security:Tokens:Key"))
                    };
                });

            //���QuartzServices
            services.AddSingleton<IJobFactory, MyJobFactory>();

            //���Job
            services.AddSingleton<MyJob>();
            services.AddSingleton(new MyJobSchedule(
                jobType: typeof(MyJob),
                cronExpression: "0/1 * * * * ?")); //ÿ��������һ��

            //ע���йܷ���
            services.AddHostedService<QuartzHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            // ������־���
            app.UseMiddleware<RequestMiddleware>();

            // ���NLog
            loggerFactory.AddNLog();

            //����Nlog�����ļ�
            NLog.LogManager.LoadConfiguration("NLog.config");
        }
    }
}
