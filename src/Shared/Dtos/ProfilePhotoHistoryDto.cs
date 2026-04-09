namespace Shared.Dtos;

public class ProfilePhotoHistoryDto
{
    public Guid Id { get; set; }
    public string PreviewUrl { get; set; } = string.Empty;
    public Guid DeficiencyId { get; set; }
    public int DeficiencyType { get; set; }
    public DateTime CreatedOn { get; set; }
}
