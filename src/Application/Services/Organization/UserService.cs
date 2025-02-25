using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Application.Providers;
using Domain.Enums;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Organization;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> RegisterAsync(User user)
    {
        user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
        user.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(0.5));

        user.RefreshToken = AuthTokensProvider.GenerateRefreshToken(user);
        user.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(3));

        await _userRepository.AddAsync(user);

        return user;
    }

    public async Task<User> AutoLoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByEmail(userLoginDto.Email) ?? throw new NullReferenceException("User was not found.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);

        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Password verification failed");

        if (user.AccessTokenExpireTime is null || user.AccessTokenExpireTime < DateTime.UtcNow)
        {
            user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
            user.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(0.5));
        }

        if (user.RefreshTokenExpireTime is null || user.RefreshTokenExpireTime < DateTime.UtcNow)
        {
            user.RefreshToken = AuthTokensProvider.GenerateRefreshToken(user);
            user.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(3));

            throw new UnauthorizedAccessException("Login to your profile, please...");
        }

        await _userRepository.UpdateAsync(user);

        return user;
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByEmail(userLoginDto.Email) ?? throw new NullReferenceException("User was not found.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);

        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Password verification failed");

        user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
        user.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(0.5));

        user.RefreshToken = AuthTokensProvider.GenerateRefreshToken(user);
        user.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(3));

        await _userRepository.UpdateAsync(user);

        return user;
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

    public async Task<User> AddUserToOrganizationAsync(Guid userId, Guid organizationId)
    {
        User user = await _userRepository.GetByIdAsync(userId) ?? throw new NullReferenceException("User was not found");

        user.OrganizationId = organizationId;

        await _userRepository.UpdateAsync(user);

        return user;
    }

    public async Task<User> AssignRoleToUserAsync(Guid userId, ERole role)
    {
        User user = await _userRepository.GetByIdAsync(userId) ?? throw new NullReferenceException("User was not found");
        user.AssignRole(role);

        await _userRepository.UpdateAsync(user);

        return user;
    }
}
