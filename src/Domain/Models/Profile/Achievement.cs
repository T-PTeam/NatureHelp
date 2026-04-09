using Domain.Enums;

namespace Domain.Models.Profile;

public class Achievement
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = "eco";
    public AchievementRuleType RuleType { get; set; }
    public int? TargetInt { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
