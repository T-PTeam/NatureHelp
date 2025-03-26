using Application.Dtos;
using Domain.Enums;
using Domain.Models.Organization;
using Shared.Dtos;

namespace Application.Interfaces.Services.Organization;
public interface IUserService
{
    public Task<User> RegisterAsync(User user);

    public Task<User> LoginAsync(UserLoginDto user);

    public Task<ListData<User>> GetOrganizationUsers(Guid organizationId, int scrollCount);
    public Task<bool> ChangeUsersRoles(Dictionary<Guid, int> changedUsersRoles);

    public bool IsTokenExpired(string token);

    public Task<User?> RefreshAccessTokenAsync(string refreshToken);


    public Task<User> AddUserToOrganizationAsync(UserLoginDto loginDto);
    public Task<IEnumerable<User>> AddMultipleUsersToOrganizationAsync(IEnumerable<User> users);
    public Task<ListData<User>> GetOrganizationUsersNotLoginEver(Guid organizationId);


}
