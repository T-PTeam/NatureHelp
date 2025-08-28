using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Application.Providers;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using Shared.Exceptions;

namespace Application.Services.Organization;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBaseService<Domain.Models.Organization.Organization> _organizationService;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly ILogger<UserService> _logger;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    public UserService(
        IUserRepository userRepository,
        IBaseService<Domain.Models.Organization.Organization> organizationService,
        ILogger<UserService> logger,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _organizationService = organizationService;
        _logger = logger;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByEmail(userLoginDto.Email) ?? throw new NullReferenceException("User was not found.");
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);

        if (result == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Password verification failed");

        user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
        user.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10));

        user.RefreshToken = AuthTokensProvider.GenerateRefreshToken(user);
        user.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(15));

        await _userRepository.UpdateAsync(user);

        return user;
    }

    public async Task<User> GetModelByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");

        return user;
    }

    public async Task<User?> ConfirmEmailByToken(string token)
    {
        User? user = await _userRepository
            .ConfirmEmailByToken(token);

        return user;
    }

    public async Task<string?> UpdateEmailConfirmationTokenByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");

        if (user.IsEmailConfirmed) return null;

        if (user.IsEmailConfirmed)
        {
            throw new InvalidOperationException("Email is already confirmed.");
        }

        user.EmailConfirmationToken = AuthTokensProvider.GenerateEmailConfirmationToken(32);
        await _userRepository.UpdateAsync(user);

        return user.EmailConfirmationToken;
    }

    public async Task<ListData<User>> GetOrganizationUsers(Guid organizationId, int scrollCount)
    {
        var users = await _userRepository.GetAllAsync(scrollCount);

        var totalCount = await _userRepository.GetTotalCount();

        var result = new ListData<User>()
        {
            List = users.Where(u => u.OrganizationId == organizationId).ToList(),
            TotalCount = totalCount,
        };

        return result;
    }

    public async Task<bool> ChangeUsersRoles(Dictionary<Guid, int> changedUsersRoles)
    {
        if (changedUsersRoles == null || !changedUsersRoles.Any())
            return false;

        var userIds = changedUsersRoles.Keys.ToList();

        var users = await _userRepository.GetAllAsync(-1);

        foreach (var user in users
            .Where(u => userIds.Contains(u.Id)))
        {
            if (changedUsersRoles.TryGetValue(user.Id, out int newRole))
            {
                user.AssignRole((ERole)newRole);
            }
        }

        await _userRepository.UpdateRangeAsync(users);

        return true;
    }

    public bool IsTokenExpired(string token)
    {
        return AuthTokensProvider.IsTokenExpired(token);
    }

    public async Task<User?> RefreshAccessTokenAsync(string refreshToken)
    {
        var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpireTime < DateTime.UtcNow)
        {
            return null;
        }

        user.AccessToken = AuthTokensProvider.GenerateAccessToken(user);
        return user;
    }

    public async Task<User> AddUserToOrganizationAsync(UserLoginDto loginDto)
    {
        if (loginDto.OrganizationId.HasValue && !(await CanAddUsersToOrganization(1, (Guid)loginDto.OrganizationId)))
        {
            _logger.LogError($"Organization ({loginDto.OrganizationId}) has reached the limit of users");
            throw new OperationCanNotBeCompleted("organizationId is not valid or organization has reached its member limit.");
        }

        User user = new User()
        {
            FirstName = loginDto.FirstName,
            LastName = loginDto.LastName,
            Email = loginDto.Email,
            Password = loginDto.Password,
            OrganizationId = loginDto.OrganizationId,
        };

        SetPasswordHash(user);

        await _userRepository.AddAsync(user);

        return user;
    }

    public async Task<IEnumerable<User>> AddMultipleUsersToOrganizationAsync(IEnumerable<User> users)
    {
        if (!(await CanAddUsersToOrganization(users.Count(), (Guid)users.First().OrganizationId!)))
        {
            _logger.LogError($"Organization ({users.First().OrganizationId}) has reached the limit of users");
            throw new OperationCanNotBeCompleted("organizationId is not valid or organization has reached its member limit.");
        }

        users = users.Select(user =>
        {
            SetPasswordHash(user);
            return user;
        });

        await _userRepository.AddRangeAsync(users);

        return users;
    }

    public async Task<ListData<User>> GetOrganizationUsersNotLoginEver(Guid organizationId)
    {
        var users = await _userRepository.GetNotLoginEver(organizationId);

        var result = new ListData<User>()
        {
            List = users.Where(u => u.OrganizationId == organizationId).ToList()
        };

        return result;
    }

    private void SetPasswordHash(User user)
    {
        user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);
    }

    private async Task<bool> CanAddUsersToOrganization(int count, Guid organizationId)
    {
        var existingUsers = await _userRepository.GetUsersCountByOrganization(organizationId);

        var organization = await _organizationService.GetByIdAsync(organizationId);

        return organization?.AllowedMembersCount - existingUsers >= count;
    }

    public async Task<bool> SendPasswordResetLinkAsync(SendPasswordResetLinkDto request)
    {
        try
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user is null)
            {
                return false;
            }

            var token = GenerateSecureToken();
            var expiry = DateTime.UtcNow.AddHours(24);

            var tokenSaved = await _userRepository.CreatePasswordResetTokenAsync(user.Id, token, expiry);
            if (!tokenSaved)
            {
                return false;
            }

            var resetLink = $"{_configuration["Frontend:Url"]}/password-reset?token={token}";
            var emailSubject = "Password Reset Request";
            var emailBody = GeneratePasswordResetEmail(user.FirstName, resetLink);

            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending password reset link: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ResetPasswordByEmailAsync(ResetPasswordDto request)
    {
        try
        {
            var userId = await ExtractUserIdFromPasswordResetToken(request.Token);

            var isTokenValid = await _userRepository.IsPasswordResetTokenValidAsync(userId, request.Token);
            if (!isTokenValid)
            {
                return false;
            }

            var hashedPassword = _passwordHasher.HashPassword(null, request.NewPassword);

            var passwordUpdated = await _userRepository.UpdatePasswordAsync(userId, hashedPassword);
            if (!passwordUpdated)
            {
                return false;
            }

            await _userRepository.InvalidatePasswordResetTokenAsync(userId);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resetting password: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ResetPasswordWithTokenAsync(ResetPasswordWithTokenDto request)
    {
        try
        {
            var userId = await _userRepository.GetUserIdByPasswordResetTokenAsync(request.ResetPasswordToken);
            if (userId is null)
            {
                return false;
            }

            var hashedPassword = _passwordHasher.HashPassword(null, request.NewPassword);

            var passwordUpdated = await _userRepository.UpdatePasswordAsync(userId ?? Guid.Empty, hashedPassword);
            return passwordUpdated;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resetting password with user ID: {ex.Message}");
            return false;
        }
    }

    private string GenerateSecureToken()
    {
        var randomBytes = new byte[32];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    private string GeneratePasswordResetEmail(string firstName, string resetLink)
    {
        return $@"
        <html>
        <body>
            <h2>Password Reset Request</h2>
            <p>Hello {firstName},</p>
            <p>You have requested to reset your password. Click the link below to proceed:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>
            <p>This link will expire in 24 hours.</p>
            <p>If you didn't request this password reset, please ignore this email.</p>
            <p>Best regards,<br>NatureHelp Team</p>
        </body>
        </html>";
    }

    public async Task<Guid> ExtractUserIdFromPasswordResetToken(string token)
    {
        var userId = await _userRepository.GetUserIdByPasswordResetTokenAsync(token);
        if (userId.HasValue)
            return userId.Value;

        throw new InvalidOperationException("Invalid or expired token.");
    }
}
