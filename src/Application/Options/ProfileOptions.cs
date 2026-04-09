namespace Application.Options;

public class ProfileOptions
{
    public const string SectionName = "Profile";

    public int DailyVisitXp { get; set; } = 5;
    public int DeficiencyApproved { get; set; } = 50;
    public string DeficiencyApprovedDescription { get; set; } = "Approved by 5 other authorized people";
    public int DeficiencyEditedXp { get; set; } = 25;
    public int PhotoAddedXp { get; set; } = 10;
}
