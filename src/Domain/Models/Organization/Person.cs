namespace Domain.Models.Organization;

public class Person : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public Location Address { get; set; } = null!;


    public string GetFullName()
    {
        return $"{LastName} {FirstName}";
    }

    public void UpdateContactDetails(string phoneNumber, Location location)
    {
        PhoneNumber = phoneNumber;
        Address = location;
    }
}
