using Application.Interfaces.Services.Organization;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
        return Ok(await _userService.RegisterAsync());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
        return Ok(await _userService.LoginAsync(user));
    }
}
