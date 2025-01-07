using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NatureHelp;
using NatureHelp.Filters;
using NatureHelp.Interfaces;
using NatureHelp.Providers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.AddScoped<IObjectsProvider<IExceptionHandler>, ErrorHandlersProvider>();

builder.Services.AddControllers(config =>
{
    config.Filters.Add<AppExceptionFilterAttribute>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.AddDbContextFactory<ApplicationContext>(options =>
{
    string connectionString = configuration.GetConnectionString("LocalConnection") ?? String.Empty;

    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("Infrastructure")
            .MinBatchSize(100)
            .MaxBatchSize(500);
    });

    if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Development"))
    {
        options.EnableSensitiveDataLogging()
            .LogTo(message => Log.Logger.Information(message), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
    }
});

builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseCors(options =>
    options.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
