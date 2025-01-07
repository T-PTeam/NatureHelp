using Shared.Models.DTOs;

namespace Application.Interfaces.Services;
public interface IUserService
{
    public Task<AuthDTO> RegisterAsync();
    public Task<AuthDTO> LoginAsync();
}
