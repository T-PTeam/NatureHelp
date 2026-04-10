using Application.Services;
using Domain.Enums;
using Domain.Models.Organization;
using Domain.Models.Profile;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Tests;
public class NatureHelpWebAppFactory : WebApplicationFactory<NatureHelp.Program>, IAsyncLifetime
{
    public static readonly Guid IntegrationTestUserId = Guid.Parse("11112222-3333-4444-5555-666677778888");
    private static readonly SemaphoreSlim SeedGate = new(1, 1);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });
    }

    public async Task InitializeAsync()
    {
        await SeedGate.WaitAsync();
        try
        {
            using var scope = Services.CreateScope();
            var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationContext>>();
            await using var db = await contextFactory.CreateDbContextAsync();
            await db.Database.EnsureCreatedAsync();

            if (!await db.DictEntries.AnyAsync(e => e.EntryKey == LevelThresholdsProvider.DictEntryKey))
            {
                db.DictEntries.Add(new AppDictEntry
                {
                    Id = Guid.NewGuid(),
                    EntryKey = LevelThresholdsProvider.DictEntryKey,
                    ValueJson = "[0,100,250,500,800,1200]",
                });
                await db.SaveChangesAsync();
            }

            if (await db.Users.AnyAsync(u => u.Id == IntegrationTestUserId))
                return;

            var user = new User
            {
                Id = IntegrationTestUserId,
                Email = "valentyn@example.com",
                FirstName = "Valentyn",
                LastName = "Integration",
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                PhoneNumber = "",
                CreatedBy = IntegrationTestUserId,
                CreatedOn = DateTime.UtcNow,
                IsEmailConfirmed = true,
            };
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "12341234");
            user.AssignRole(ERole.Owner);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }
        finally
        {
            SeedGate.Release();
        }
    }

    Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;

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
