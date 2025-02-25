using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

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
}
