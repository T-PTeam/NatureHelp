using Application.Providers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NatureHelp;
using NatureHelp.Filters;
using NatureHelp.Interfaces;
using NatureHelp.Providers;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("AspNetCore_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
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
    string connectionString = configuration.GetConnectionString("LocalConnection") ?? String.Empty;

    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("Infrastructure")
            .MinBatchSize(100)
            .MaxBatchSize(500);

        /* To add migration open src folder and run the following command:
            dotnet ef migrations add InitialCreate --project Infrastructure\Infrastructure.csproj --startup-project NatureHelp\NatureHelp.csproj --output-dir Migrations */

        /* dotnet ef database update --project Infrastructure\Infrastructure.csproj --startup-project NatureHelp\NatureHelp.csproj */
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
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
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
});

builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
