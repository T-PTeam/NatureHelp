using Domain.Enums;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class ProductionSuperAdminBootstrapper : IProductionSuperAdminBootstrapper
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProductionSuperAdminBootstrapper> _logger;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public ProductionSuperAdminBootstrapper(
        IDbContextFactory<ApplicationContext> contextFactory,
        IConfiguration configuration,
        ILogger<ProductionSuperAdminBootstrapper> logger)
    {
        _contextFactory = contextFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task EnsureSuperAdminAsync(CancellationToken cancellationToken = default)
    {
        var email = _configuration["SuperAdmin:Email"];
        var password = _configuration["SuperAdmin:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning(
                "SuperAdmin bootstrap skipped: set SuperAdmin:Email and SuperAdmin:Password (e.g. environment variables).");
            return;
        }

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var normalized = email.Trim();
        if (await context.Users.AnyAsync(u => u.Email == normalized, cancellationToken))
        {
            return;
        }

        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            Email = normalized,
            FirstName = "Super",
            LastName = "Admin",
            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            PhoneNumber = "",
            CreatedBy = id,
            CreatedOn = DateTime.UtcNow,
            IsEmailConfirmed = true,
            OrganizationId = null,
            LaboratoryId = null,
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        user.AssignRole(ERole.SuperAdmin);
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Created SuperAdmin user for email {Email}.", normalized);
    }
}
