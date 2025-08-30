using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public override async Task<IEnumerable<User>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            if (scrollCount == -1)
            {
                IQueryable<User> fullList = context.Set<User>()
                    .Include(u => u.Organization)
                    .Include(u => u.Laboratory);

                foreach (var user in fullList)
                {
                    if (user.Laboratory != null) user.Laboratory.Researchers = null;
                }

                return await fullList.ToListAsync();
            }

            IQueryable<User> list = context.Set<User>()
                    .Include(u => u.Organization)
                    .Include(u => u.Laboratory)
                    .Skip(scrollCount * 20).Take(20);

            foreach (var user in list)
            {
                if (user.Laboratory != null) user.Laboratory.Researchers = null;
            }

            return await list
                .ToListAsync();
        }
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<User>()
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Email == email);
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
            return await context.Set<User>().FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }

    public async Task<IEnumerable<User>> GetNotLoginEver(Guid? organizationId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            if (organizationId == null)
                return await context.Set<User>().Where(u => string.IsNullOrEmpty(u.RefreshToken)).ToArrayAsync();

            return await context.Set<User>()
                .Where(u => string.IsNullOrEmpty(u.RefreshToken)
                    && u.OrganizationId == organizationId)
                .ToArrayAsync();
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

}
