namespace Domain.Models.Organization;

public class Person : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Location Address { get; set; } = null!;
}
