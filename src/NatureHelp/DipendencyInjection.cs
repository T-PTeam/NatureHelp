using Application.Interfaces.Services.Nature;
using Application.Interfaces.Services.Organization;
using Application.Services.Nature;
using Application.Services.Organization;
using Domain.Interfaces;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace NatureHelp
{
    public static class DipendencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IBaseRepository<WaterDeficiency>, BaseRepository<WaterDeficiency>>();
            services.AddScoped<IBaseRepository<SoilDeficiency>, BaseRepository<SoilDeficiency>>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeficiencyService, DeficiencyService>();
            services.AddScoped<ILaboratoryService, LaboratoryService>();
            services.AddScoped<IOrganizationService, OrganizationService>();

            return services;
        }
    }
}
