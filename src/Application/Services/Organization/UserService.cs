using Application.Interfaces.Services;
using Core.Interfaces;
using Core.Models.Organization;
using Shared.Models.DTOs;

namespace Application.Services.Organization;
public class UserService : IUserService
{
    private readonly IBaseRepository<User> _userRepository;
    public UserService() { }

    public Task<AuthDTO> LoginAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AuthDTO> RegisterAsync()
    {
        throw new NotImplementedException();
    }
}
