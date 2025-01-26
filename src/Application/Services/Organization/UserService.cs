using Application.Interfaces.Services.Organization;
using Application.Providers;
using Domain.Models.Organization;
using Infrastructure.Interfaces;

namespace Application.Services.Organization;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> RegisterAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<User> LoginAsync(User user)
    {
        user = await _userRepository.GetUserByCredentials(user.Email, user.PasswordHash) ?? user;

        if (user.AccessTokenExpireTime is null || user.AccessTokenExpireTime < DateTime.UtcNow)
        {
            user.AccessToken = AuthTokenProvider.MakeAccessToken(user.Email);
            user.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(0.5));
        }

        if (user.RefreshTokenExpireTime is null || user.RefreshTokenExpireTime < DateTime.UtcNow)
        {
            user.RefreshToken = AuthTokenProvider.MakeRefreshToken(user.Email);
            user.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(3));
        }

        await _userRepository.UpdateAsync(user);

        return user;
    }
}
