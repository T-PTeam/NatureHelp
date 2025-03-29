using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Application.Providers;
using Domain.Enums;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Dtos;

namespace Application.Services.Organization;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByEmail(userLoginDto.Email) ?? throw new NullReferenceException("User was not found.");
        var result = PasswordVerificationResult.Failed;

        if (string.IsNullOrEmpty(userLoginDto.PasswordHash))
            result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);
        else if (userLoginDto.PasswordHash == user.PasswordHash) result = PasswordVerificationResult.Success;


        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Password verification failed");

        user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
        user.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(0.5));

        user.RefreshToken = AuthTokensProvider.GenerateRefreshToken(user);
        user.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(3));

        await _userRepository.UpdateAsync(user);

        return user;
    }

    public async Task<ListData<User>> GetOrganizationUsers(Guid organizationId, int scrollCount)
    {
        var users = await _userRepository.GetAllAsync(scrollCount);

        var totalCount = await _userRepository.GetTotalCount();

        var result = new ListData<User>()
        {
            List = users.Where(u => u.OrganizationId == organizationId).ToList(),
            TotalCount = totalCount,
        };

        return result;
    }

    public async Task<bool> ChangeUsersRoles(Dictionary<Guid, int> changedUsersRoles)
    {
        if (changedUsersRoles == null || !changedUsersRoles.Any())
            return false;

        var userIds = changedUsersRoles.Keys.ToList();

        var users = await _userRepository.GetAllAsync(-1);

        foreach (var user in users
            .Where(u => userIds.Contains(u.Id)))
        {
            if (changedUsersRoles.TryGetValue(user.Id, out int newRole))
            {
                user.AssignRole((ERole)newRole);
            }
        }

        await _userRepository.UpdateRangeAsync(users);

        return true;
    }

    public bool IsTokenExpired(string token)
    {
        return AuthTokensProvider.IsTokenExpired(token);
    }

    public async Task<User?> RefreshAccessTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpireTime < DateTime.UtcNow)
        {
            return null;
        }

        user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
        return user;
    }

    private void SetPasswordHash(User user)
    {
        user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);
    }

    public async Task<User> AddUserToOrganizationAsync(UserLoginDto loginDto)
    {
        try
        {
            User user = new User()
            {
                FirstName = loginDto.FirstName,
                LastName = loginDto.LastName,
                Email = loginDto.Email,
                Password = loginDto.Password,
                OrganizationId = loginDto.OrganizationId,
            };

            SetPasswordHash(user);

            await _userRepository.AddAsync(user);

            return user;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<User>> AddMultipleUsersToOrganizationAsync(IEnumerable<User> users)
    {
        users = users.Select(user =>
        {
            SetPasswordHash(user);
            return user;
        });

        await _userRepository.AddRangeAsync(users);

        return users;
    }

    public async Task<ListData<User>> GetOrganizationUsersNotLoginEver(Guid organizationId)
    {
        var users = await _userRepository.GetNotLoginEver(organizationId);

        var result = new ListData<User>()
        {
            List = users.Where(u => u.OrganizationId == organizationId).ToList()
        };

        return result;
    }
}
