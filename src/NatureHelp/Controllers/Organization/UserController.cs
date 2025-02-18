using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Register user
    /// </summary>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync()
    {
        return Ok(await _userService.RegisterAsync());
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(UserLoginDto user)
    {
        return Ok(await _userService.LoginAsync(user));
    }

}
