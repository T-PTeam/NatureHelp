using Domain.Interfaces;
using Domain.Models.Organization;

namespace Infrastructure.Interfaces;
public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> GetUserByEmail(string email);
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
}
