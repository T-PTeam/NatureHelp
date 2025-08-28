using Application.Dtos;
using Domain.Models.Organization;
using Shared.Dtos;

namespace Application.Interfaces.Services.Organization;
public interface IUserService
{
    public Task<User> LoginAsync(UserLoginDto user);
    public Task<User> GetModelByEmail(string email);
    public Task<User?> ConfirmEmailByToken(string emailConfirmationToken);

    public Task<ListData<User>> GetOrganizationUsers(Guid organizationId, int scrollCount);
    public Task<bool> ChangeUsersRoles(Dictionary<Guid, int> changedUsersRoles);

    public bool IsTokenExpired(string token);

    public Task<User?> RefreshAccessTokenAsync(string refreshToken);
    public Task<string?> UpdateEmailConfirmationTokenByEmail(string email);

    public Task<User> AddUserToOrganizationAsync(UserLoginDto loginDto);
    public Task<IEnumerable<User>> AddMultipleUsersToOrganizationAsync(IEnumerable<User> users);
    public Task<ListData<User>> GetOrganizationUsersNotLoginEver(Guid organizationId);

    Task<bool> SendPasswordResetLinkAsync(SendPasswordResetLinkDto request);
    Task<bool> ResetPasswordByEmailAsync(ResetPasswordDto request);
    Task<bool> ResetPasswordWithTokenAsync(ResetPasswordWithTokenDto request);
    public Task<Guid> ExtractUserIdFromPasswordResetToken(string token);

}
