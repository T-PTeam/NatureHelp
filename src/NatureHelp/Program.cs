using Application.Providers;
using AspNetCoreRateLimit;
using Azure.Storage.Blobs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Models;
using NatureHelp;
using NatureHelp.Filters;
using NatureHelp.Interfaces;
using NatureHelp.Providers;
using NatureHelp.Security;
using Npgsql;
using Serilog;
using StackExchange.Redis;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    allowedOrigins = builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("CI")
        ? ["http://localhost:4200", "http://localhost:5051", "http://localhost:3000"]
        : Array.Empty<string>();
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddMemoryCache();

var dataProtectionBuilder = builder.Services.AddDataProtection()
    .SetApplicationName("NatureHelp");

var keysPath = Path.Combine(builder.Environment.ContentRootPath, "DataProtection-Keys");
if (!Directory.Exists(keysPath))
{
    Directory.CreateDirectory(keysPath);
}
dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(keysPath));

builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContextFactory<ApplicationContext>(options =>
        options.UseInMemoryDatabase("NatureHelpIntegrationTests"));
}
else
{
    builder.Services.AddDbContextFactory<ApplicationContext>(options =>
    {
        string connectionString = builder.Configuration.GetConnectionString("LocalConnection")
            ?? builder.Configuration.GetConnectionString("DefaultConnection")
            ?? String.Empty;

        Log.Information("Database connection string: {ConnectionString}",
            connectionString.Replace("Password=10101010", "Password=***"));

        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly("Infrastructure")
                .MinBatchSize(100)
                .MaxBatchSize(500)
                .EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);

        });

        if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").Equals("Development"))
        {
            options.EnableSensitiveDataLogging()
                .LogTo(message => Log.Logger.Information(message), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
        }
    });
}

var authenticationBuilder = builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/api/user/login";
        options.LogoutPath = "/api/user/logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("CI"))
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            options.Cookie.SameSite = SameSiteMode.None;
        }
        else
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
        }
    })
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

if (!builder.Environment.IsEnvironment("Testing") && !builder.Environment.IsEnvironment("CI"))
{
    authenticationBuilder
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration["OAuth2:Google:ClientId"] ?? string.Empty;
            options.ClientSecret = builder.Configuration["OAuth2:Google:ClientSecret"] ?? string.Empty;
            options.CallbackPath = "/api/user/signin-google";
            options.SaveTokens = true;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.UsePkce = true;
            if (builder.Environment.IsDevelopment())
            {
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;
                options.CorrelationCookie.SameSite = SameSiteMode.None;
            }
            else
            {
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.CorrelationCookie.SameSite = SameSiteMode.None;
            }
        })
        .AddFacebook(options =>
        {
            options.AppId = builder.Configuration["OAuth2:Facebook:AppId"] ?? string.Empty;
            options.AppSecret = builder.Configuration["OAuth2:Facebook:AppSecret"] ?? string.Empty;
            options.CallbackPath = "/api/user/signin-facebook";
            options.Scope.Add("email");
            options.Fields.Add("name");
            options.Fields.Add("email");
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            if (builder.Environment.IsDevelopment())
            {
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;
                options.CorrelationCookie.SameSite = SameSiteMode.None;
            }
            else
            {
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.CorrelationCookie.SameSite = SameSiteMode.None;
            }
        });
}

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

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

builder.Services.AddInfrastructureServices(builder.Configuration);
if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("CI"))
{
    builder.Services.AddScoped<IDevelopmentDatabaseSeeder, DevelopmentDatabaseSeeder>();
}

builder.Services.AddScoped<IProductionSuperAdminBootstrapper, ProductionSuperAdminBootstrapper>();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        ConnectionMultiplexer.Connect("127.0.0.1:6379,abortConnect=false"));
    builder.Services.AddSingleton(_ =>
        new BlobServiceClient("UseDevelopmentStorage=true"));
}
else
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    {
        var configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
        if (string.IsNullOrEmpty(configuration))
            throw new InvalidOperationException("Redis connection string is not configured.");

        return ConnectionMultiplexer.Connect(configuration);
    });

    builder.Services.AddSingleton(_ =>
    {
        var blobConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
        return new BlobServiceClient(blobConnectionString);
    });
}

builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing") && !app.Environment.IsEnvironment("CI"))
{
    app.UseHttpsRedirection();
}

if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing") && !app.Environment.IsEnvironment("CI"))
{
    app.UseHsts();
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("Strict-Transport-Security", 
            "max-age=31536000; includeSubDomains; preload");
        await next();
    });
}
// else
// {
//    // In Development, do not send HSTS to avoid forcing HTTPS on local HTTP ports
//     app.Use(async (context, next) =>
//     {
//         context.Response.Headers.Append("Strict-Transport-Security", "max-age=3600; includeSubDomains");
//         await next();
//     });
// }

app.UseCors("AllowSpecificOrigins");

app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseIpRateLimiting();
app.UseHttpMetrics();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("CI"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "NatureHelp v2");
    });
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("CI"))
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var contextFactory = services.GetRequiredService<IDbContextFactory<ApplicationContext>>();
            using var context = contextFactory.CreateDbContext();

            Log.Information("Applying database migrations...");
            context.Database.Migrate();
            Log.Information("Database migrations applied successfully.");
            var devSeeder = services.GetRequiredService<IDevelopmentDatabaseSeeder>();
            await devSeeder.SeedAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }
}
else if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var bootstrapper = services.GetRequiredService<IProductionSuperAdminBootstrapper>();
            await bootstrapper.EnsureSuperAdminAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "SuperAdmin bootstrap failed.");
        }
    }
}

app.Run();
