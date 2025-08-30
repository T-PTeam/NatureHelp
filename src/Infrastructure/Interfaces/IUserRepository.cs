using Domain.Interfaces;
using Domain.Models.Organization;

namespace Infrastructure.Interfaces;
public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> GetUserByEmail(string email);
    public Task<User?> ConfirmEmailByToken(string emailConfirmationToken);
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

    public Task<IEnumerable<User>> GetNotLoginEver(Guid? organizationId);
    public Task<int> GetUsersCountByOrganization(Guid organizationId);

    Task<bool> UpdatePasswordAsync(Guid userId, string hashedPassword);
    Task<bool> CreatePasswordResetTokenAsync(Guid userId, string token, DateTime expiry);
    Task<string?> GetPasswordResetTokenAsync(Guid userId);
    Task<bool> InvalidatePasswordResetTokenAsync(Guid userId);
    Task<bool> IsPasswordResetTokenValidAsync(Guid userId, string token);
    public Task<Guid?> GetUserIdByPasswordResetTokenAsync(string token);

}
