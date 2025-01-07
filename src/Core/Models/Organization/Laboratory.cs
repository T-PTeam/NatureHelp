namespace Core.Models.Organization;

public class Laboratory
{
    public string Caption { get; set; } = null!;
    public List<User>? Researchers { get; set; }
}
