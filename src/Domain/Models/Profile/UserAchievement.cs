using Domain.Models.Organization;

namespace Domain.Models.Profile;

public class UserAchievement
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid AchievementId { get; set; }
    public Achievement Achievement { get; set; } = null!;
    public int Progress { get; set; }
    public DateTime? UnlockedAt { get; set; }
    public bool IsCompleted { get; set; }
}
