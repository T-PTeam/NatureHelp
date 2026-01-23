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
using Serilog;
using StackExchange.Redis;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? [])
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

builder.Services.AddAuthentication(options =>
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
        if (builder.Environment.IsDevelopment())
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
    })
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

builder.Services.AddSingleton(x =>
{
    var blobConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
    return new BlobServiceClient(blobConnectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("Strict-Transport-Security", 
            "max-age=31536000; includeSubDomains; preload");
        await next();
    });
}
else
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=3600; includeSubDomains");
        await next();
    });
}

app.UseCors("AllowSpecificOrigins");

app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseIpRateLimiting();
app.UseHttpMetrics();

if (app.Environment.IsDevelopment())
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
app.MapMetrics();

if (app.Environment.IsDevelopment())
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
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }
}

app.Run();
