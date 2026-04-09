namespace Shared.Dtos;

public class UserProfileSettingsDto
{
    public bool ProfileIsPublic { get; set; } = true;
    public bool EmailNotificationsEnabled { get; set; } = true;
    public bool AchievementAlertsEnabled { get; set; } = true;
    public bool NewsletterEnabled { get; set; }
}
