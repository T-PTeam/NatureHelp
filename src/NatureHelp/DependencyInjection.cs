using Application.Interfaces.Services;
using Application.Interfaces.Services.Analitycs;
using Application.Interfaces.Services.Organization;
using Application.Services.Analitycs;
using Application.Services.Organization;
using Domain.Interfaces;
using Domain.Models.Analitycs;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace NatureHelp
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IBaseRepository<WaterDeficiency>, DeficiencyRepository<WaterDeficiency>>();
            services.AddScoped<IBaseRepository<SoilDeficiency>, DeficiencyRepository<SoilDeficiency>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBaseRepository<Laboratory>, LaboratoryRepository>();
            services.AddScoped<IBaseRepository<Research>, ResearchRepository>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseService<Laboratory>, BaseService<Laboratory>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExcelExportService, ExcelExportService>();

            services.AddScoped<IBaseService<User>, BaseService<User>>();
            services.AddScoped<IBaseService<WaterDeficiency>, BaseService<WaterDeficiency>>();
            services.AddScoped<IBaseService<SoilDeficiency>, BaseService<SoilDeficiency>>();
            services.AddScoped<IBaseService<Laboratory>, BaseService<Laboratory>>();
            services.AddScoped<IBaseService<Research>, BaseService<Research>>();


            return services;
        }
    }
}
