namespace Shared.Dtos;

public class ProfileAchievementDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int RuleType { get; set; }
    public int? TargetInt { get; set; }
    public int SortOrder { get; set; }
    public int Progress { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? UnlockedAt { get; set; }
    public int Kind { get; set; }
}
