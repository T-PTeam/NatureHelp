namespace Shared.Dtos;

public class ProfileJournalEntryDto
{
    public Guid Id { get; set; }
    public int DeficiencyType { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
