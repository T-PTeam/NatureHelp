using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Application.Providers;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using Shared.Exceptions;

namespace Application.Services.Organization;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBaseService<Domain.Models.Organization.Organization> _organizationService;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly ILogger<UserService> _logger;
    public UserService(
        IUserRepository userRepository,
        IBaseService<Domain.Models.Organization.Organization> organizationService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _organizationService = organizationService;
        _logger = logger;
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByEmail(userLoginDto.Email) ?? throw new NullReferenceException("User was not found.");
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);

        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Password verification failed");

        user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
        user.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10));

        user.RefreshToken = AuthTokensProvider.GenerateRefreshToken(user);
        user.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(15));

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

    public async Task<User> AddUserToOrganizationAsync(UserLoginDto loginDto)
    {
        if (loginDto.OrganizationId.HasValue && !(await CanAddUsersToOrganization(1, (Guid)loginDto.OrganizationId)))
        {
            _logger.LogError($"Organization ({loginDto.OrganizationId}) has reached the limit of users");
            throw new OperationCanNotBeCompleted("organizationId is not valid or organization has reached its member limit.");
        }

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

    public async Task<IEnumerable<User>> AddMultipleUsersToOrganizationAsync(IEnumerable<User> users)
    {
        if (!(await CanAddUsersToOrganization(users.Count(), (Guid)users.First().OrganizationId!)))
        {
            _logger.LogError($"Organization ({users.First().OrganizationId}) has reached the limit of users");
            throw new OperationCanNotBeCompleted("organizationId is not valid or organization has reached its member limit.");
        }

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

    private void SetPasswordHash(User user)
    {
        user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);
    }

    private async Task<bool> CanAddUsersToOrganization(int count, Guid organizationId)
    {
        var existingUsers = await _userRepository.GetUsersCountByOrganization(organizationId);

        var organization = await _organizationService.GetByIdAsync(organizationId);

        return organization?.AllowedMembersCount - existingUsers >= count;
    }
}
