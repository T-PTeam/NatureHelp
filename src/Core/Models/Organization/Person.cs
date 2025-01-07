namespace Core.Models.Organization;

public class Person
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Location Address { get; set; } = null!;
}
