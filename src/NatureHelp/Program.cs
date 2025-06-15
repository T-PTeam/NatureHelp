using Application.Providers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NatureHelp;
using NatureHelp.Filters;
using NatureHelp.Interfaces;
using NatureHelp.Providers;
using Serilog;
using StackExchange.Redis;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.AddDbContextFactory<ApplicationContext>(options =>
{
    string connectionString = configuration.GetConnectionString("LocalConnection")
        ?? configuration.GetConnectionString("DefaultConnection")
        ?? String.Empty;

    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("Infrastructure")
            .MinBatchSize(100)
            .MaxBatchSize(500);

        /* To add migration open src folder and run the following command:
            dotnet ef migrations add InitialCreate --project Infrastructure\Infrastructure.csproj --startup-project NatureHelp\NatureHelp.csproj --output-dir Migrations */

        /* To Update DB
        
        dotnet ef database update --project Infrastructure\Infrastructure.csproj --startup-project NatureHelp\NatureHelp.csproj */

        /* To generate SQL Script (choose previous migration ID)
        
        dotnet ef migrations script -i 20250319083430_Rewriting_Initial_Create --project Infrastructure\Infrastructure.csproj--startup - project NatureHelp\NatureHelp.csproj--output Infrastructure\Migrations\SQL\Autogenerating_Data.sql */
    });

    if ((Environment.GetEnvironmentVariable("AspNetCore_ENVIRONMENT") ?? "Development").Equals("Development"))
    {
        options.EnableSensitiveDataLogging()
            .LogTo(message => Log.Logger.Information(message), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
    }
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = AuthTokensProvider.ISSUER,
            ValidAudience = AuthTokensProvider.AUDIENCE,
            IssuerSigningKey = AuthTokensProvider.GetSecurityKey(),

            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IObjectsProvider<IExceptionHandler>, ErrorHandlersProvider>();

builder.Services.AddControllers(config =>
    {
        config.Filters.Add<AppExceptionFilterAttribute>();
    })
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    }); ;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "NatureHelp",
        Description = "Swagger API controlling of ERP monitoring system",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Write authorization JWT token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        }
                    },
                    Array.Empty<string>()
                }
            });

    options.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
    if (string.IsNullOrEmpty(configuration))
        throw new InvalidOperationException("Redis connection string is not configured.");

    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

app.UseCors(options =>
options.AllowAnyHeader()
.AllowAnyOrigin()
.AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "NatureHelp v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
