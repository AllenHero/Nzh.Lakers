using Microsoft.Extensions.DependencyInjection;
using Nzh.Lakers.Cache.Interface;
using Nzh.Lakers.Cache.MemoryCache;
using Nzh.Lakers.Cache.RedisCache;
using Nzh.Lakers.IRepository;
using Nzh.Lakers.IService;
using Nzh.Lakers.Repository;
using Nzh.Lakers.Service;
using System;

namespace Nzh.Lakers.Factory
{
    public static class FactoryContainer
    {
        /// <summary>
        /// 注入服务、仓储类
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddScoped<IDemoService, DemoService>();
            services.AddScoped<IDemoRepository, DemoRepository>();

            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ITestRepository, TestRepository>();

            services.AddScoped<IEnclosureRepository, EnclosureRepository>();
            services.AddScoped<IEnclosureService, EnclosureService>();

            services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
