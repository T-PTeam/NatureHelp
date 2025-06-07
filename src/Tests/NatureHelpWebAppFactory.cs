using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
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
            logging.AddConsole(); // <-- Додає вивід у консоль
            logging.SetMinimumLevel(LogLevel.Information); // або Debug для більше деталей
        });

        builder.ConfigureServices(services =>
        {
            // Видаляємо всі реєстрації ApplicationContext (і DbContextFactory, і DbContext)
            var descriptors = services.Where(s =>
                s.ServiceType == typeof(DbContextOptions<ApplicationContext>) ||
                s.ServiceType == typeof(IDbContextFactory<ApplicationContext>) ||
                s.ServiceType == typeof(ApplicationContext)
            ).ToList();

            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            // Додаємо InMemory замість PostgreSQL
            services.AddDbContextFactory<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Побудова scope для ініціалізації БД
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            db.Database.EnsureCreated();
        });
    }
}
