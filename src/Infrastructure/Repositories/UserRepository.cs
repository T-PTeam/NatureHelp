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
                    .Include(u => u.Address)
                    .Include(u => u.Organization)
                    .Include(u => u.Laboratory);

                foreach (var user in fullList)
                {
                    if (user.Laboratory != null) user.Laboratory.Researchers = null;
                }

                return await fullList.ToListAsync();
            }

            IQueryable<User> list = context.Set<User>()
                    .Include(u => u.Address)
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
            return await context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
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
                return await context.Set<User>().Where(u => string.IsNullOrEmpty(u.AccessToken)).ToArrayAsync();

            return await context.Set<User>()
                .Where(u => string.IsNullOrEmpty(u.AccessToken)
                    && u.OrganizationId == organizationId)
                .ToArrayAsync();
        }
    }
}
