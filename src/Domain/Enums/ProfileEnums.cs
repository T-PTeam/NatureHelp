namespace Domain.Enums;

public enum XpReason
{
    DailyVisit = 0,
    DeficiencyApproved = 1,
    DeficiencyEdited = 2,
    PhotoAddedToDeficiency = 3,
}

public enum AchievementRuleType
{
    TotalReportsCount = 0,
    WaterReportsCount = 1,
    SoilReportsCount = 2,
    TotalXp = 3,
    CurrentLevel = 4,
    ConfirmationsGivenCount = 5,
    DeficiencyGotFiveConfirmations = 6,
}
