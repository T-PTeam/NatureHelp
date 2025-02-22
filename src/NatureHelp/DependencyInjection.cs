using Application.Interfaces.Services.Analitycs;
using Application.Interfaces.Services.Nature;
using Application.Interfaces.Services.Organization;
using Application.Services.Analitycs;
using Application.Services.Nature;
using Application.Services.Organization;
using Domain.Interfaces;
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
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IDeficiencyRepository<WaterDeficiency>, DeficiencyRepository<WaterDeficiency>>();
            services.AddScoped<IDeficiencyRepository<SoilDeficiency>, DeficiencyRepository<SoilDeficiency>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILaboratoryRepository, LaboratoryRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeficiencyService, DeficiencyService>();
            services.AddScoped<ILaboratoryService, LaboratoryService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
