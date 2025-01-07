using Application.Interfaces.Services;
using Application.Services.Organization;
using Core.Interfaces;
using Core.Models.Organization;
using Infrastructure.Repositories;

namespace NatureHelp
{
    public static class DipendencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
