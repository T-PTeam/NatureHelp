using Application.Dtos;
using Application.Interfaces.Services;
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
    private readonly IProfileRepository _profileRepository;
    private readonly IProfileService _profileService;
    private readonly IAchievementEvaluationService _achievementEvaluation;

    public UserService(
        IUserRepository userRepository,
        IBaseService<Domain.Models.Organization.Organization> organizationService,
        ILogger<UserService> logger,
        IEmailService emailService,
        IConfiguration configuration,
        IProfileRepository profileRepository,
        IProfileService profileService,
        IAchievementEvaluationService achievementEvaluation)
    {
        _userRepository = userRepository;
        _organizationService = organizationService;
        _logger = logger;
        _emailService = emailService;
        _configuration = configuration;
        _profileRepository = profileRepository;
        _profileService = profileService;
        _achievementEvaluation = achievementEvaluation;
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

    public async Task<ListData<User>> GetOrganizationUsers(
        Guid organizationId,
        int scrollCount,
        IDictionary<string, string?>? filters = null)
    {
        var users = await _userRepository.GetByOrganizationAsync(organizationId, scrollCount, filters);
        var totalCount = await _userRepository.GetTotalCountByOrganization(organizationId);

        return new ListData<User>()
        {
            List = users.ToList(),
            TotalCount = totalCount,
        };
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

    public async Task<User> AddUserToOrganizationAsync(UserLoginDto loginDto, bool isCreatingOwner)
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

        user.AssignRole(isCreatingOwner ? ERole.Owner : ERole.Supervisor);

        SetPasswordHash(user);

        await _userRepository.AddAsync(user);
        await NotifyNewOrganizationUsersAsync([user]);

        return user;
    }

    public async Task<IEnumerable<User>> AddMultipleUsersToOrganizationAsync(IEnumerable<User> users)
    {
        var usersList = users.ToList();

        if (!(await CanAddUsersToOrganization(usersList.Count, (Guid)usersList.First().OrganizationId!)))
        {
            _logger.LogError($"Organization ({usersList.First().OrganizationId}) has reached the limit of users");
            throw new OperationCanNotBeCompleted("organizationId is not valid or organization has reached its member limit.");
        }

        usersList = usersList.Select(user =>
        {
            SetPasswordHash(user);
            return user;
        }).ToList();

        await _userRepository.AddRangeAsync(usersList);
        await NotifyNewOrganizationUsersAsync(usersList);

        return usersList;
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

    private async Task NotifyNewOrganizationUsersAsync(IEnumerable<User> users)
    {
        if (!IsEmailConfigured())
        {
            _logger.LogWarning("Email settings are not configured. Skipping organization invite notifications.");
            return;
        }

        var usersList = users.ToList();
        if (usersList.Count == 0)
        {
            return;
        }

        var organizationId = usersList.First().OrganizationId;
        if (!organizationId.HasValue)
        {
            return;
        }

        var organization = await _organizationService.GetByIdAsync(organizationId.Value);
        var organizationTitle = organization?.Title ?? "your organization";
        var loginUrl = $"{_configuration["Frontend:Url"]?.TrimEnd('/')}/login";

        foreach (var user in usersList)
        {
            await TrySendOrganizationInviteEmailAsync(user, organizationTitle, loginUrl);
        }
    }

    private async Task TrySendOrganizationInviteEmailAsync(User user, string organizationTitle, string loginUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return;
            }

            var plainPassword = user.Password;
            if (string.IsNullOrWhiteSpace(plainPassword))
            {
                _logger.LogWarning("Skipping invite email for {Email} because no initial password was provided.", user.Email);
                return;
            }

            var subject = $"You've been invited to {organizationTitle} on NatureHelp";
            var body = GenerateOrganizationInviteEmail(
                user.FirstName,
                organizationTitle,
                user.Email,
                plainPassword,
                loginUrl,
                user.Role);

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send organization invite email to {Email}", user.Email);
        }
    }

    private bool IsEmailConfigured()
    {
        return !string.IsNullOrWhiteSpace(_configuration["EmailSettings:Username"])
            && !string.IsNullOrWhiteSpace(_configuration["EmailSettings:From"]);
    }

    private static string GenerateOrganizationInviteEmail(
        string firstName,
        string organizationTitle,
        string email,
        string password,
        string loginUrl,
        ERole role)
    {
        return $@"
        <html>
        <body>
            <h2>Welcome to NatureHelp</h2>
            <p>Hello {firstName},</p>
            <p>You have been added to <strong>{organizationTitle}</strong> as a <strong>{role}</strong>.</p>
            <p>Use the credentials below to sign in:</p>
            <ul>
                <li><strong>Email:</strong> {email}</li>
                <li><strong>Password:</strong> {password}</li>
            </ul>
            <p><a href='{loginUrl}'>Open NatureHelp login</a></p>
            <p>Please change your password after your first login.</p>
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

    public async Task<User> LoginOrRegisterWithOAuth2Async(string email, string firstName, string lastName, string provider)
    {
        var existingUser = await _userRepository.GetUserByEmail(email);
        
        if (existingUser != null)
        {
            existingUser.AccessToken = AuthTokensProvider.GenerateAccessToken(existingUser);
            existingUser.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10));
            existingUser.RefreshToken = AuthTokensProvider.GenerateRefreshToken(existingUser);
            existingUser.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(15));
            
            await _userRepository.UpdateAsync(existingUser);
            return existingUser;
        }
        
        var newUser = new User()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            IsEmailConfirmed = true, 
            OrganizationId = null, 
        };
        
        newUser.AssignRole(ERole.Supervisor);
        
        newUser.Password = Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks.ToString() + provider;
        SetPasswordHash(newUser);
        
        newUser.AccessToken = AuthTokensProvider.GenerateAccessToken(newUser);
        newUser.AccessTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10));
        newUser.RefreshToken = AuthTokensProvider.GenerateRefreshToken(newUser);
        newUser.RefreshTokenExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(15));
        
        await _userRepository.AddAsync(newUser);
        
        _logger.LogInformation($"New user registered via OAuth2 {provider}: {email}");
        
        return newUser;
    }

    public async Task<bool> DeleteUserDataAsync(string email)
    {
        try
        {
            var result = await _userRepository.DeleteUserDataAsync(email);
            if (result)
            {
                _logger.LogInformation($"User data deleted for email: {email}");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user data for email: {email}");
            return false;
        }
    }

    public async Task<UserProfileStatsDto> GetProfileStatsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        var (water, soil) = await _userRepository.CountCreatedDeficienciesAsync(user.Id);
        var total = water + soil;
        var referralsCount = await _profileRepository.CountReferralsAsync(user.Id);
        var (level, xpInto, xpToNext) = await _profileService.GetLevelProgressAsync(user.TotalXp);

        var stage = level switch
        {
            <= 2 => "seed",
            <= 4 => "sprout",
            <= 6 => "sapling",
            _ => "tree",
        };

        var statusName = level switch
        {
            1 => "Seed Keeper",
            2 => "Seedling",
            3 => "Sprout",
            4 => "Sapling",
            5 => "Ranger",
            6 => "Guardian",
            7 => "Warden",
            8 => "Elder Tree",
            _ => "Nature Elder",
        };

        return new UserProfileStatsDto
        {
            ReportsCount = total,
            WaterReportsCount = water,
            SoilReportsCount = soil,
            ReferralsCount = referralsCount,
            TotalXp = user.TotalXp,
            Level = level,
            XpCurrent = xpInto,
            XpToNextLevel = xpToNext,
            AvatarStage = stage,
            StatusName = statusName,
        };
    }

    public async Task<IReadOnlyList<ProfileJournalEntryDto>> GetProfileJournalAsync(string email, int take = 100, int skip = 0, int? deficiencyType = null)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        return await _profileRepository.GetProfileJournalAsync(user.Id, take, skip, deficiencyType);
    }

    public async Task<IReadOnlyList<ProfilePhotoHistoryDto>> GetProfilePhotoHistoryAsync(string email, int take = 100)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        var list = await _profileRepository.GetAttachmentsByCreatorAsync(user.Id, take);
        return list.Select(a => new ProfilePhotoHistoryDto
        {
            Id = a.Id,
            PreviewUrl = a.PreviewUrl ?? string.Empty,
            DeficiencyId = a.DeficiencyId,
            DeficiencyType = (int)a.DeficiencyType,
            CreatedOn = a.CreatedOn,
        }).ToList();
    }

    public async Task<IReadOnlyList<ProfileAchievementDto>> GetProfileAchievementsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        await _achievementEvaluation.EvaluateForUserAsync(user.Id);
        var achievements = await _profileRepository.GetActiveAchievementsAsync();
        var result = new List<ProfileAchievementDto>();
        foreach (var ach in achievements)
        {
            var ua = await _profileRepository.GetUserAchievementAsync(user.Id, ach.Id);
            result.Add(new ProfileAchievementDto
            {
                Id = ach.Id,
                Code = ach.Code,
                Title = ach.Title,
                Description = ach.Description,
                Icon = ach.Icon,
                RuleType = (int)ach.RuleType,
                TargetInt = ach.TargetInt,
                SortOrder = ach.SortOrder,
                Progress = ua?.Progress ?? 0,
                IsCompleted = ua?.IsCompleted ?? false,
                UnlockedAt = ua?.UnlockedAt,
            });
        }

        return result;
    }

    public async Task<bool> RecordProfileVisitAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        return await _profileService.TryAwardDailyVisitAsync(user.Id);
    }

    public async Task<User> UpdateProfileSettingsAsync(string email, UserProfileSettingsDto settings)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        user.ProfileIsPublic = settings.ProfileIsPublic;
        user.EmailNotificationsEnabled = settings.EmailNotificationsEnabled;
        user.AchievementAlertsEnabled = settings.AchievementAlertsEnabled;
        user.NewsletterEnabled = settings.NewsletterEnabled;
        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task<IReadOnlyList<ProfileReferralDto>> GetProfileReferralsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        return await _profileRepository.GetReferralsAsync(user.Id);
    }

    public async Task<string> GetReferralInviteLinkAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email) ?? throw new NullReferenceException("User was not found.");
        var code = await _profileRepository.EnsureReferralCodeAsync(user.Id);
        var baseUrl = _configuration["Frontend:Url"]?.TrimEnd('/') ?? string.Empty;
        return $"{baseUrl}/register?ref={code}";
    }
}
