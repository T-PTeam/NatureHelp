namespace Shared.Dtos;

public class UserProfileStatsDto
{
    public int ReportsCount { get; set; }
    public int WaterReportsCount { get; set; }
    public int SoilReportsCount { get; set; }
    public int ReferralsCount { get; set; }
    public int TotalXp { get; set; }
    public int Level { get; set; }
    public int XpCurrent { get; set; }
    public int XpToNextLevel { get; set; }
    public string AvatarStage { get; set; } = "seed";
    public string StatusName { get; set; } = string.Empty;
}
