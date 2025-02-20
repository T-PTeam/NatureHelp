using Application.Dtos;
using Domain.Models.Organization;

namespace Application.Interfaces.Services.Organization;
public interface IUserService
{
    public Task<User> RegisterAsync(User user);
    public Task<User> LoginAsync(UserLoginDto user);

    public bool IsTokenExpired(string token);

    public Task<User?> RefreshAccessTokenAsync(string refreshToken);
}
