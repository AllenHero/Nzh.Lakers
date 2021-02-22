using Microsoft.Extensions.DependencyInjection;
using Nzh.Lakers.Cache.Interface;
using Nzh.Lakers.Cache.MemoryCache;
using Nzh.Lakers.Cache.RedisCache;
using Nzh.Lakers.IRepository;
using Nzh.Lakers.IRepository.SystemManagement;
using Nzh.Lakers.IService;
using Nzh.Lakers.IService.SystemManagement;
using Nzh.Lakers.Repository;
using Nzh.Lakers.Repository.SystemManagement;
using Nzh.Lakers.Service;
using Nzh.Lakers.Service.SystemManagement;
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

            services.AddScoped<ISysButtonRepository, SysButtonRepository>();
            services.AddScoped<ISysButtonService, SysButtonService>();

            services.AddScoped<ISysDepartmentRepository, SysDepartmentRepository>();
            services.AddScoped<ISysDepartmentService, SysDepartmentService>();

            services.AddScoped<ISysDictRepository, SysDictRepository>();
            services.AddScoped<ISysDictService, SysDictService>();

            services.AddScoped<ISysLogRepository, SysLogRepository>();
            services.AddScoped<ISysLogService, SysLogService>();

            services.AddScoped<ISysMenuButtonMapRepository, SysMenuButtonMapRepository>();

            services.AddScoped<ISysMenuRepository, SysMenuRepository>();
            services.AddScoped<ISysMenuService, SysMenuService>();

            services.AddScoped<ISysPositionRepository, SysPositionRepository>();
            services.AddScoped<ISysPositionService, SysPositionService>();

            services.AddScoped<ISysRoleMenuMapRepository, SysRoleMenuMapRepository>();

            services.AddScoped<ISysRoleRepository, SysRoleRepository>();
            services.AddScoped<ISysRoleService, SysRoleService>();

            services.AddScoped<ISysUserRepository, SysUserRepository>();
            services.AddScoped<ISysUserService, SysUserService>();

            services.AddScoped<ISysUserRoleMapRepository, SysUserRoleMapRepository>();

            services.AddScoped<IUserToken, UserToken>();

            services.AddScoped<IUserHelper, UserHelper>();

            return services;
        }
    }
}
