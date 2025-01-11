namespace Domain.Models.Organization;

public class Organization : BaseEntity
{
    public string Title { get; set; } = null!;
    public Location Location { get; set; } = null!;
    public List<User>? Staff { get; set; }
}
