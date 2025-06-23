using Application.Interfaces.Services;
using Application.Interfaces.Services.Analitycs;
using Application.Interfaces.Services.Audit;
using Application.Interfaces.Services.Cache;
using Application.Interfaces.Services.Organization;
using Application.Services;
using Application.Services.Analitycs;
using Application.Services.Audit;
using Application.Services.Cache;
using Application.Services.Nature;
using Application.Services.Organization;
using Domain.Interfaces;
using Domain.Models.Analitycs;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Infrastructure.Providers;
using Infrastructure.Repositories;

namespace NatureHelp
{
    public partial class Program { }

    /// <summary>
    /// Connecting DI to Program.cs
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
            services.AddScoped(typeof(IDeficiencyBindModelRepository<>), typeof(DeficiencyBindModelRepository<>));

            services.AddScoped<IBaseRepository<WaterDeficiency>, DeficiencyRepository<WaterDeficiency>>();
            services.AddScoped<IBaseRepository<SoilDeficiency>, DeficiencyRepository<SoilDeficiency>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBaseRepository<Laboratory>, LaboratoryRepository>();
            services.AddScoped<IBaseRepository<Research>, ResearchRepository>();
            services.AddScoped<IChangedModelLogRepository, ChangedModelLogRepository>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();

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
            services.AddScoped(typeof(IModelByDeficiencyService<>), typeof(DeficiencyBindModelService<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseService<Laboratory>, BaseService<Laboratory>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExcelExportService, ExcelExportService>();

            services.AddScoped<IBaseService<User>, BaseService<User>>();
            services.AddScoped<IBaseService<WaterDeficiency>, WaterDeficiencyService>();
            services.AddScoped<IBaseService<SoilDeficiency>, SoilDeficiencyService>();
            services.AddScoped<IBaseService<Laboratory>, BaseService<Laboratory>>();
            services.AddScoped<IBaseService<Research>, BaseService<Research>>();
            services.AddScoped<IChangedModelLogService, ChangedModelLogService>();
            services.AddScoped<IBlobStorageProvider, AzureBlobStorageProvider>();

            return services;
        }
    }
}
