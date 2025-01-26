namespace Domain.Models.Organization;

public class Laboratory : BaseModel
{
    public string Title { get; set; } = null!;
    public List<User>? Researchers { get; set; }
    public Location? Location { get; set; }
    public Guid LocationId { get; set; }
}
