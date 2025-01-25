using Domain.Interfaces;
using Domain.Models.Organization;

namespace Infrastructure.Interfaces;
public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> GetUserByCredentials(string email, string passwordHash);
}
