using Domain.Enums;
using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public override async Task<IEnumerable<User>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters, User? currentUser = null)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            if (scrollCount == -1)
            {
                IQueryable<User> fullList = context.Set<User>()
                    .Include(u => u.Organization)
                    .Include(u => u.UserLaboratories)
                    .ThenInclude(ul => ul.Laboratory);

                return PopulateLaboratories(await fullList.ToListAsync());
            }

            IQueryable<User> list = context.Set<User>()
                    .Include(u => u.Organization)
                    .Include(u => u.UserLaboratories)
                    .ThenInclude(ul => ul.Laboratory)
                    .Skip(scrollCount * 20).Take(20);

            return PopulateLaboratories(await list.ToListAsync());
        }
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Set<User>()
                .Include(u => u.Organization)
                .Include(u => u.UserLaboratories)
                .ThenInclude(ul => ul.Laboratory)
                .FirstOrDefaultAsync(u => u.Email == email);

            return user == null ? null : PopulateLaboratories(user);
        }
    }

    public async Task<User?> ConfirmEmailByToken(string emailConfirmationToken)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Set<User>()
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.EmailConfirmationToken == emailConfirmationToken);

            if (user is not null)
            {
                user.IsEmailConfirmed = true;
                user.EmailConfirmationToken = null;

                await context.SaveChangesAsync();
            }

            return user;
        }
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Set<User>()
                .Include(u => u.UserLaboratories)
                .ThenInclude(ul => ul.Laboratory)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            return user == null ? null : PopulateLaboratories(user);
        }
    }

    public async Task<IEnumerable<User>> GetNotLoginEver(Guid? organizationId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            if (organizationId == null)
            {
                var usersWithoutLogin = await context.Set<User>()
                    .Where(u => string.IsNullOrEmpty(u.RefreshToken))
                    .Include(u => u.UserLaboratories)
                    .ThenInclude(ul => ul.Laboratory)
                    .ToArrayAsync();

                return PopulateLaboratories(usersWithoutLogin).ToArray();
            }

            var users = await context.Set<User>()
                .Include(u => u.UserLaboratories)
                .ThenInclude(ul => ul.Laboratory)
                .Where(u => string.IsNullOrEmpty(u.RefreshToken)
                    && u.OrganizationId == organizationId)
                .ToArrayAsync();

            return PopulateLaboratories(users).ToArray();
        }
    }

    public async Task<int> GetUsersCountByOrganization(Guid organizationId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<User>()
                .Where(u => u.OrganizationId == organizationId)
                .CountAsync();
        }
    }

    public async Task<bool> UpdatePasswordAsync(Guid userId, string hashedPassword)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.PasswordHash = hashedPassword;

            var result = await context.SaveChangesAsync();
            return result > 0;
        }

    }

    public async Task<bool> CreatePasswordResetTokenAsync(Guid userId, string token, DateTime expiry)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = expiry;

            var result = await context.SaveChangesAsync();
            return result > 0;
        }
    }

    public async Task<string?> GetPasswordResetTokenAsync(Guid userId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users
        .Where(u => u.Id == userId)
        .Select(u => u.PasswordResetToken)
        .FirstOrDefaultAsync();

            return user;
        }
    }

    public async Task<bool> InvalidatePasswordResetTokenAsync(Guid userId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            var result = await context.SaveChangesAsync();
            return result > 0;
        }
    }

    public async Task<bool> IsPasswordResetTokenValidAsync(Guid userId, string token)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.PasswordResetToken, u.PasswordResetTokenExpiry })
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            return user.PasswordResetToken == token &&
                   user.PasswordResetTokenExpiry.HasValue &&
                   user.PasswordResetTokenExpiry.Value > DateTime.UtcNow;
        }
    }

    public async Task<Guid?> GetUserIdByPasswordResetTokenAsync(string token)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.PasswordResetToken == token && u.PasswordResetTokenExpiry > DateTime.UtcNow);

            return user?.Id;
        }
    }

    public async Task<bool> DeleteUserDataAsync(string email)
    {
        var user = await GetUserByEmail(email);
        
        if (user == null)
            return false;

        await DeleteAsync(user.Id);
        return true;
    }

    public async Task<(int WaterCount, int SoilCount)> CountCreatedDeficienciesAsync(Guid userId)
    {
        using var context = _contextFactory.CreateDbContext();
        var water = await context.WaterDeficiencies.AsNoTracking().CountAsync(d => d.CreatedBy == userId);
        var soil = await context.SoilDeficiencies.AsNoTracking().CountAsync(d => d.CreatedBy == userId);
        return (water, soil);
    }

    public async Task<IEnumerable<User>> GetByOrganizationAsync(
        Guid organizationId,
        int scrollCount,
        IDictionary<string, string?>? filters = null)
    {
        using var context = _contextFactory.CreateDbContext();

        var (sortBy, sortDirection, remainingFilters) = QueryableExtensions.ExtractSorting(filters);
        var query = context.Set<User>()
            .Where(u => u.OrganizationId == organizationId)
            .Include(u => u.UserLaboratories)
            .ThenInclude(ul => ul.Laboratory)
            .ApplyFilters(remainingFilters)
            .ApplySorting(sortBy, sortDirection);

        if (scrollCount != -1)
        {
            query = query.Skip(scrollCount * 20).Take(20);
        }

        return PopulateLaboratories(await query.ToListAsync());
    }

    public async Task<int> GetTotalCountByOrganization(Guid organizationId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Set<User>().CountAsync(u => u.OrganizationId == organizationId);
    }

    public async Task<List<User>> GetResearchersByOrganizationAsync(Guid organizationId)
    {
        using var context = _contextFactory.CreateDbContext();
        var users = await context.Set<User>()
            .Where(u => u.OrganizationId == organizationId && u.Role == ERole.Researcher)
            .Include(u => u.UserLaboratories)
            .ThenInclude(ul => ul.Laboratory)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync();

        return PopulateLaboratories(users).ToList();
    }

    public async Task SyncLaboratoryResearchersAsync(Guid laboratoryId, Guid organizationId, IEnumerable<Guid> researcherIds)
    {
        using var context = _contextFactory.CreateDbContext();
        var normalizedResearcherIds = researcherIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        var eligibleResearcherIds = await context.Set<User>()
            .Where(u => u.OrganizationId == organizationId && u.Role == ERole.Researcher && normalizedResearcherIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync();

        var existingMemberships = await context.UserLaboratories
            .Where(ul => ul.LaboratoryId == laboratoryId)
            .ToListAsync();

        var existingUserIds = existingMemberships.Select(ul => ul.UserId).ToHashSet();
        var eligibleUserIds = eligibleResearcherIds.ToHashSet();

        context.UserLaboratories.RemoveRange(existingMemberships.Where(ul => !eligibleUserIds.Contains(ul.UserId)));

        var newMemberships = eligibleUserIds
            .Except(existingUserIds)
            .Select(userId => new UserLaboratory
            {
                UserId = userId,
                LaboratoryId = laboratoryId,
            });

        await context.UserLaboratories.AddRangeAsync(newMemberships);
        await context.SaveChangesAsync();

        var affectedUserIds = existingUserIds.Union(eligibleUserIds).ToList();
        if (affectedUserIds.Count == 0)
        {
            return;
        }

        var membershipsByUser = await context.UserLaboratories
            .Where(ul => affectedUserIds.Contains(ul.UserId))
            .OrderBy(ul => ul.LaboratoryId)
            .ToListAsync();

        var users = await context.Users
            .Where(u => affectedUserIds.Contains(u.Id))
            .ToListAsync();

        foreach (var user in users)
        {
            user.LaboratoryId = membershipsByUser
                .Where(ul => ul.UserId == user.Id)
                .Select(ul => (Guid?)ul.LaboratoryId)
                .FirstOrDefault();
        }

        await context.SaveChangesAsync();
    }

    private static IEnumerable<User> PopulateLaboratories(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            PopulateLaboratories(user);
        }

        return users;
    }

    private static User PopulateLaboratories(User user)
    {
        var laboratories = user.UserLaboratories?
            .Select(ul => ul.Laboratory)
            .Where(l => l != null)
            .OrderBy(l => l!.Title)
            .Select(l =>
            {
                return new Laboratory
                {
                    Id = l!.Id,
                    Title = l.Title,
                };
            })
            .ToList() ?? new List<Laboratory>();

        user.Laboratories = laboratories!;
        user.Laboratory = laboratories.FirstOrDefault();
        if (user.LaboratoryId == null)
        {
            user.LaboratoryId = user.Laboratory?.Id;
        }

        return user;
    }

}

