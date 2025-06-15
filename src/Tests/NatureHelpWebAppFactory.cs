using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests;
public class NatureHelpWebAppFactory : WebApplicationFactory<NatureHelp.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(s =>
                s.ServiceType == typeof(DbContextOptions<ApplicationContext>) ||
                s.ServiceType == typeof(IDbContextFactory<ApplicationContext>) ||
                s.ServiceType == typeof(ApplicationContext)
            ).ToList();

            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            services.AddDbContextFactory<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            db.Database.EnsureCreated();
        });
    }

    public WebApplicationFactory<NatureHelp.Program> WithTestAuth(string role)
    {
        TestAuthHandler.Role = role;

        return this.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(TestAuthHandler.SchemeName)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            TestAuthHandler.SchemeName, options => { });

                services.PostConfigureAll<AuthenticationOptions>(opts =>
                {
                    opts.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    opts.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                });

                services.AddAuthorization();
            });
        });
    }
}
