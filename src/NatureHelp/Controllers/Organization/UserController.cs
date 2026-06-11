using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Security.Claims;
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
    [Authorize(Roles = "SuperAdmin,Owner,Manager,Supervisor,Researcher")]
    [HttpGet("organization-users")]
    public async Task<IActionResult> GetOrganizationUsers(
        [FromQuery] Guid organizationId,
        [FromQuery] int scrollCount,
        [FromQuery] IDictionary<string, string?>? filters)
    {
        return Ok(await _userService.GetOrganizationUsers(organizationId, scrollCount, filters));
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
        try
        {
            var loggedInUser = await _userService.LoginAsync(user);
            
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = loggedInUser.AccessTokenExpireTime ?? DateTime.UtcNow.Add(TimeSpan.FromMinutes(10))
            };
            
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = loggedInUser.RefreshTokenExpireTime ?? DateTime.UtcNow.Add(TimeSpan.FromDays(15))
            };
            
            Response.Cookies.Append("accessToken", loggedInUser.AccessToken ?? string.Empty, accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", loggedInUser.RefreshToken ?? string.Empty, refreshTokenCookieOptions);
            
            return Ok(loggedInUser);
        }
        catch (NullReferenceException ex)
        {
            return Unauthorized(new ErrorResponseDto
            {
                Message = "Invalid email or password. Please try again.",
                StatusCode = 401,
                ErrorType = "AuthenticationError"
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ErrorResponseDto
            {
                Message = "Invalid email or password. Please try again.",
                StatusCode = 401,
                ErrorType = "AuthenticationError"
            });
        }
    }

    /// <summary>
    /// Used for autologin for users that were authorized previously
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpPost("current-user")]
    public async Task<IActionResult> GetCurrentUserAsync([FromBody] UserDto user)
    {
        return Ok(await _userService.GetModelByEmail(user.Email));
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpGet("profile-stats")]
    [HttpGet("profile/stats")]
    public async Task<IActionResult> GetProfileStats()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        return Ok(await _userService.GetProfileStatsAsync(email));
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpGet("profile/referrals")]
    public async Task<IActionResult> GetProfileReferrals()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        return Ok(await _userService.GetProfileReferralsAsync(email));
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpGet("profile/referral-invite-link")]
    public async Task<IActionResult> GetReferralInviteLink()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        var link = await _userService.GetReferralInviteLinkAsync(email);
        return Ok(new { inviteLink = link });
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpGet("profile/journal")]
    public async Task<IActionResult> GetProfileJournal([FromQuery] int take = 100, [FromQuery] int skip = 0, [FromQuery] int? deficiencyType = null)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        return Ok(await _userService.GetProfileJournalAsync(email, take, skip, deficiencyType));
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpGet("profile/photo-history")]
    public async Task<IActionResult> GetProfilePhotoHistory([FromQuery] int take = 100)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        return Ok(await _userService.GetProfilePhotoHistoryAsync(email, take));
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpGet("profile/achievements")]
    public async Task<IActionResult> GetProfileAchievements()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        return Ok(await _userService.GetProfileAchievementsAsync(email));
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpPost("profile/visit")]
    public async Task<IActionResult> RecordProfileVisit()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        var awarded = await _userService.RecordProfileVisitAsync(email);
        return Ok(new { awarded });
    }

    [Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor, Researcher")]
    [HttpPut("profile-settings")]
    public async Task<IActionResult> UpdateProfileSettings([FromBody] UserProfileSettingsDto settings)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();
        return Ok(await _userService.UpdateProfileSettingsAsync(email, settings));
    }

    /// <summary>
    /// Check token expiration and availability
    /// </summary>
    /// <param name="tokensDto"></param>
    /// <returns>User options with new access token</returns>
    [HttpPost("refresh-access-token")]
    public async Task<IActionResult> RefreshAccessTokenAsync([FromBody] TokensDto tokensDto)
    {
       var refreshToken = tokensDto.RefreshToken ?? Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Token was not found...");

        var user = await _userService.RefreshAccessTokenAsync(refreshToken);
        
        if (user == null) return Unauthorized("Invalid or expired refresh token");
        
        var accessTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = user.AccessTokenExpireTime ?? DateTime.UtcNow.Add(TimeSpan.FromMinutes(10))
        };
        
        Response.Cookies.Append("accessToken", user.AccessToken ?? string.Empty, accessTokenCookieOptions);
        
        return Ok(user);
    }

    /// <summary>
    /// Logout user and clear credential cookies
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(-1)
        };
        
        Response.Cookies.Append("accessToken", string.Empty, cookieOptions);
        Response.Cookies.Append("refreshToken", string.Empty, cookieOptions);
        
        return Ok(new { message = "Logged out successfully" });
    }

    /// <summary>
    /// Initiate Google OAuth2 login
    /// </summary>
    /// <returns></returns>
    [HttpGet("login-google")]
    [AllowAnonymous]
    public IActionResult LoginWithGoogle()
    {
        var redirectUrl = Url.Action(nameof(GoogleCallback), "User", null, Request.Scheme);
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Google OAuth2 callback handler
    /// </summary>
    /// <returns></returns>
    [HttpGet("signin-google")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            return Redirect($"{_configuration["Frontend:Url"]}/login?error=oauth_failed");
        }

        var claims = result.Principal?.Claims.ToList();
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            ?? claims?.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        var firstName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;
        var lastName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? string.Empty;
        var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;

        if (string.IsNullOrEmpty(email))
        {
            return Redirect($"{_configuration["Frontend:Url"]}/login?error=email_not_provided");
        }

        if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(name))
        {
            var nameParts = name.Split(' ', 2);
            firstName = nameParts[0];
            lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
        }

        var user = await _userService.LoginOrRegisterWithOAuth2Async(email, firstName, lastName, "Google");

        var accessTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = user.AccessTokenExpireTime ?? DateTime.UtcNow.Add(TimeSpan.FromMinutes(10))
        };

        var refreshTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = user.RefreshTokenExpireTime ?? DateTime.UtcNow.Add(TimeSpan.FromDays(15))
        };

        Response.Cookies.Append("accessToken", user.AccessToken ?? string.Empty, accessTokenCookieOptions);
        Response.Cookies.Append("refreshToken", user.RefreshToken ?? string.Empty, refreshTokenCookieOptions);

        await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);

        return Redirect($"{_configuration["Frontend:Url"]}/dashboard?oauth=google");
    }

    /// <summary>
    /// Initiate Facebook OAuth2 login
    /// </summary>
    /// <returns></returns>
    [HttpGet("login-facebook")]
    [AllowAnonymous]
    public IActionResult LoginWithFacebook()
    {
        var redirectUrl = Url.Action(nameof(FacebookCallback), "User", null, Request.Scheme);
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, FacebookDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// Facebook OAuth2 callback handler
    /// </summary>
    /// <returns></returns>
    [HttpGet("signin-facebook")]
    [AllowAnonymous]
    public async Task<IActionResult> FacebookCallback()
    {
        var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            return Redirect($"{_configuration["Frontend:Url"]}/login?error=oauth_failed");
        }

        var claims = result.Principal?.Claims.ToList();
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var firstName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;
        var lastName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? string.Empty;
        var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;

        if (string.IsNullOrEmpty(email))
        {
            return Redirect($"{_configuration["Frontend:Url"]}/login?error=email_not_provided");
        }

        if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(name))
        {
            var nameParts = name.Split(' ', 2);
            firstName = nameParts[0];
            lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
        }

        var user = await _userService.LoginOrRegisterWithOAuth2Async(email, firstName, lastName, "Facebook");

        var accessTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = user.AccessTokenExpireTime ?? DateTime.UtcNow.Add(TimeSpan.FromMinutes(10))
        };

        var refreshTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = user.RefreshTokenExpireTime ?? DateTime.UtcNow.Add(TimeSpan.FromDays(15))
        };

        Response.Cookies.Append("accessToken", user.AccessToken ?? string.Empty, accessTokenCookieOptions);
        Response.Cookies.Append("refreshToken", user.RefreshToken ?? string.Empty, refreshTokenCookieOptions);

        await HttpContext.SignOutAsync(FacebookDefaults.AuthenticationScheme);

        return Redirect($"{_configuration["Frontend:Url"]}/dashboard?oauth=facebook");
    }

    /// <summary>
    /// Send verification email to user
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("send-verification-email")]
    public async Task<IActionResult> SendVerificationEmail([FromBody] UserDto userDto)
    {
        var user = await _userService.GetModelByEmail(userDto.Email);

        if (user == null) return NotFound("User not found");
        if (user.IsEmailConfirmed) return Accepted("Email already confirmed" as object);

        string? token = await _userService.UpdateEmailConfirmationTokenByEmail(userDto.Email);
        user.EmailConfirmationToken = token;

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
    [Authorize(Roles = "SuperAdmin, Owner")]
    [HttpPost("add-new-to-org")]
    public async Task<IActionResult> AddNewUserToOrganizationAsync([FromBody] UserLoginDto loginDto)
    {
        return Ok(await _userService.AddUserToOrganizationAsync(loginDto, loginDto.IsCreatingOwner));
    }

    /// <summary>
    /// Add multiple users to organization
    /// </summary>
    /// <param name="users"></param>
    /// <returns>Assigning role to user</returns>
    [Authorize(Roles = "SuperAdmin, Owner")]
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
    [Authorize(Roles = "SuperAdmin, Owner")]
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

    [HttpDelete("delete-user-data")]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteUserData([FromBody] UserDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Email))
        {
            return BadRequest(new { success = false, message = "Email is required." });
        }

        try
        {
            var result = await _userService.DeleteUserDataAsync(userDto.Email);

            if (result)
            {
                return Ok(new { success = true, message = "User data has been successfully deleted." });
            }
            else
            {
                return NotFound(new { success = false, message = "User not found." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
        }
    }
}
