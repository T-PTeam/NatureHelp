using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Domain.Enums;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;

[AllowAnonymous]
[Route("api/[controller]")]
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
    [HttpPost("autologin")]
    public async Task<IActionResult> AutoLoginAsync([FromBody] UserLoginDto user)
    {
        return Ok(await _userService.AutoLoginAsync(user));
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto user)
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
    public IActionResult CheckTokenExpiration([FromBody] string token)
    {
        return Ok(_userService.IsTokenExpired(token));
    }

    /// <summary>
    /// Check token expiration and availability
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>User options with new access token</returns>
    [HttpPost("refresh-access-token")]
    public async Task<IActionResult> RefreshAccessTokenAsync([FromBody] string refreshToken)
    {
        return Ok(await _userService.RefreshAccessTokenAsync(refreshToken));
    }

    /// <summary>
    /// Check token expiration and availability
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="role"></param>
    /// <returns>Assigning role to user</returns>
    [HttpGet("assign-role/{userId}")]
    public async Task<IActionResult> AssignRoleToUserAsync([FromRoute] Guid userId, [FromQuery] ERole role)
    {
        return Ok(await _userService.AssignRoleToUserAsync(userId, role));
    }

    /// <summary>
    /// Add user to organization
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="organizationId"></param>
    /// <returns>Assigning role to user</returns>
    [HttpGet("add-to-organization/{userId}")]
    public async Task<IActionResult> AddUserToOrganizationAsync([FromRoute] Guid userId, [FromQuery] Guid organizationId)
    {
        return Ok(await _userService.AddUserToOrganizationAsync(userId, organizationId));
    }
}
