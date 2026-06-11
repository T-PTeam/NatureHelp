using Application.Dtos;
using Domain.Models.Organization;
using Shared.Dtos;

namespace Application.Interfaces.Services.Organization;
public interface IUserService
{
    public Task<User> LoginAsync(UserLoginDto user);
    public Task<User> GetModelByEmail(string email);
    public Task<User?> ConfirmEmailByToken(string emailConfirmationToken);

    public Task<ListData<User>> GetOrganizationUsers(
        Guid organizationId,
        int scrollCount,
        IDictionary<string, string?>? filters = null);
    public Task<bool> ChangeUsersRoles(Dictionary<Guid, int> changedUsersRoles);

    public bool IsTokenExpired(string token);

    public Task<User?> RefreshAccessTokenAsync(string refreshToken);
    public Task<string?> UpdateEmailConfirmationTokenByEmail(string email);

    public Task<User> AddUserToOrganizationAsync(UserLoginDto loginDto, bool isCreatingOwner);
    public Task<IEnumerable<User>> AddMultipleUsersToOrganizationAsync(IEnumerable<User> users);
    public Task<ListData<User>> GetOrganizationUsersNotLoginEver(Guid organizationId);

    Task<bool> SendPasswordResetLinkAsync(SendPasswordResetLinkDto request);
    Task<bool> ResetPasswordByEmailAsync(ResetPasswordDto request);
    Task<bool> ResetPasswordWithTokenAsync(ResetPasswordWithTokenDto request);
    public Task<Guid> ExtractUserIdFromPasswordResetToken(string token);
    
    Task<User> LoginOrRegisterWithOAuth2Async(string email, string firstName, string lastName, string provider);
    
    Task<bool> DeleteUserDataAsync(string email);

    Task<UserProfileStatsDto> GetProfileStatsAsync(string email);

    Task<User> UpdateProfileSettingsAsync(string email, UserProfileSettingsDto settings);

    Task<IReadOnlyList<ProfileJournalEntryDto>> GetProfileJournalAsync(string email, int take = 100, int skip = 0, int? deficiencyType = null);

    Task<IReadOnlyList<ProfilePhotoHistoryDto>> GetProfilePhotoHistoryAsync(string email, int take = 100);

    Task<IReadOnlyList<ProfileAchievementDto>> GetProfileAchievementsAsync(string email);

    Task<bool> RecordProfileVisitAsync(string email);

    Task<IReadOnlyList<ProfileReferralDto>> GetProfileReferralsAsync(string email);
    Task<string> GetReferralInviteLinkAsync(string email);
}

