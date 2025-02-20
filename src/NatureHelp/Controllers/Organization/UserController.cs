using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Domain.Models.Organization;
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
    public async Task<IActionResult> RegisterAsync(User user)
    {
        return Ok(await _userService.RegisterAsync(user));
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

    /// <summary>
    /// Check token expiration and availability
    /// </summary>
    /// <param name="token"></param>
    /// <returns>true - is expired</returns>
    /// <returns>false - working</returns>
    [HttpPost("check-token-expiration")]
    public IActionResult CheckTokenExpiration(string token)
    {
        return Ok(_userService.IsTokenExpired(token));
    }

    /// <summary>
    /// Check token expiration and availability
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>User options with new access token</returns>
    [HttpPost("refresh-access-token")]
    public async Task<IActionResult> RefreshAccessTokenAsync(string refreshToken)
    {
        return Ok(await _userService.RefreshAccessTokenAsync(refreshToken));
    }
}
