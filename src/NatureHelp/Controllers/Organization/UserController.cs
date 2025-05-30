using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

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
    /// Get organization users
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Owner, Manager")]
    [HttpGet("organization-users")]
    public async Task<IActionResult> GetOrganizationUsers([FromQuery] Guid organizationId, [FromQuery] int scrollCount)
    {
        return Ok(await _userService.GetOrganizationUsers(organizationId, scrollCount));

    }

    /// <summary>
    /// Get organization users
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Owner")]
    [HttpPut("users-roles")]
    public async Task<IActionResult> ChangeUsersRoles([FromBody] Dictionary<Guid, int> changedUsersRoles)
    {
        return Ok(await _userService.ChangeUsersRoles(changedUsersRoles));
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
    /// <param name="tokensDto"></param>
    /// <returns>User options with new access token</returns>
    [HttpPost("refresh-access-token")]
    public async Task<IActionResult> RefreshAccessTokenAsync([FromBody] TokensDto tokensDto)
    {
        if (string.IsNullOrEmpty(tokensDto.RefreshToken)) return BadRequest("Token was not found...");

        return Ok(await _userService.RefreshAccessTokenAsync(tokensDto.RefreshToken!));
    }

    /// <summary>
    /// Add user to organization
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns>Assigning role to user</returns>
    [Authorize(Roles = "Owner")]
    [HttpPost("add-new-to-org")]
    public async Task<IActionResult> AddNewUserToOrganizationAsync([FromBody] UserLoginDto loginDto)
    {
        return Ok(await _userService.AddUserToOrganizationAsync(loginDto));
    }

    /// <summary>
    /// Add multiple users to organization
    /// </summary>
    /// <param name="users"></param>
    /// <returns>Assigning role to user</returns>
    [Authorize(Roles = "Owner")]
    [HttpPost("add-multiple-to-org")]
    public async Task<IActionResult> AddMultipleUsersToOrganizationAsync([FromBody] IEnumerable<User> users)
    {
        return Ok(await _userService.AddMultipleUsersToOrganizationAsync(users));
    }

    /// <summary>
    /// Get users that were not login ever
    /// </summary>
    /// <param name="organizationId"></param>
    /// <returns>Assigning role to user</returns>
    [Authorize(Roles = "Owner")]
    [HttpGet("users-not-login-ever")]
    public async Task<IActionResult> GetOrganizationUsersNotLoginEver([FromQuery] Guid organizationId)
    {
        return Ok(await _userService.GetOrganizationUsersNotLoginEver(organizationId));
    }
}
