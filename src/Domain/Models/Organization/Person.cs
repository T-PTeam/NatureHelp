﻿namespace Domain.Models.Organization;

public class Person : BaseModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }

    public string GetFullName()
    {
        return $"{LastName} {FirstName}";
    }

    public void UpdateContactDetails(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
}
