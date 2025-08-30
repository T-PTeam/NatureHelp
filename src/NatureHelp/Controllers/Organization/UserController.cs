using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace NatureHelp.Controllers.Organization;

[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailSender;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService,
        IEmailService emailSender,
        IConfiguration configuration)
    {
        _userService = userService;
        _emailSender = emailSender;
        _configuration = configuration;
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
    /// Send verification email to user
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("send-verification-email")]
    public async Task<IActionResult> SendVerificationEmail([FromBody] UserDto userDto)
    {
        string? token = await _userService.UpdateEmailConfirmationTokenByEmail(userDto.Email);

        var user = await _userService.GetModelByEmail(userDto.Email);
        user.EmailConfirmationToken = token;

        if (user == null) return NotFound("User not found");
        if (user.IsEmailConfirmed) return Accepted("Email already confirmed");

        var confirmationUrl = $"{_configuration["Frontend:Url"]}/confirm-email?token={user.EmailConfirmationToken}";

        await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your email by clicking the following link: {confirmationUrl}");

        return Ok("Confirmation email sent.");
    }


    /// <summary>
    /// Confirm user email
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string token)
    {
        var user = await _userService.ConfirmEmailByToken(token);

        if (user == null || !user.IsEmailConfirmed) return BadRequest("Invalid token");

        return Ok("Email confirmed");
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

    /// <summary>
    /// Send password reset link to user's email
    /// </summary>
    /// <param name="request">Email address to send reset link to</param>
    /// <returns>Success status</returns>
    [HttpPost("send-password-reset-link")]
    public async Task<IActionResult> SendPasswordResetLink([FromBody] SendPasswordResetLinkDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userService.SendPasswordResetLinkAsync(request);

            return Ok(new { success = true, message = "If the email exists, a password reset link has been sent." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Reset password using token from email
    /// </summary>
    /// <param name="request">Token and new password</param>
    /// <returns>Success status</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userService.ResetPasswordByEmailAsync(request);

            if (result)
            {
                return Ok(new { success = true, message = "Password has been successfully reset." });
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid or expired reset token." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Reset password using user ID (for authenticated users)
    /// </summary>
    /// <param name="request">User ID and new password</param>
    /// <returns>Success status</returns>
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPasswordWithUserId([FromBody] ResetPasswordWithTokenDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userService.ResetPasswordWithTokenAsync(request);

            if (result)
            {
                return Ok(new { success = true, message = "Password has been successfully reset." });
            }
            else
            {
                return BadRequest(new { success = false, message = "User not found or password reset failed." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
        }
    }
}
