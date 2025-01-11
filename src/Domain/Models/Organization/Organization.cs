namespace Core.Models.Organization;

public class Organization
{
    public string Caption { get; set; } = null!;
    public Location Location { get; set; } = null!;
    public List<User>? Staff { get; set; }
}
