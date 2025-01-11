namespace Domain.Models.Organization;

public class Laboratory : BaseEntity
{
    public string Title { get; set; } = null!;
    public List<User>? Researchers { get; set; }
}
