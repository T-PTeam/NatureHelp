using Domain.Enums;
using Domain.Models.Organization;
using FluentAssertions;

namespace Tests.UnitTests.Domain;
public class UserTests
{
    [Theory]
    [InlineData("", false)]
    [InlineData("plainaddress", false)]
    [InlineData("@missingusername.com", false)]
    [InlineData("username@.com", false)]
    [InlineData("username@com", false)]
    [InlineData("username@domain.com", true)]
    [InlineData("user.name+tag@sub.domain.co", true)]
    [InlineData("user_name@domain.co.uk", true)]
    public void UserEmailChecking(string email, bool expectedResult)
    {
        var user = new User();
        user.IsEmailValid(email).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("plainaddress", false)]
    [InlineData("@missingusername.com", false)]
    [InlineData("username@.com", false)]
    [InlineData("username@com", false)]
    [InlineData("username@domain.com", true)]
    [InlineData("user.name+tag@sub.domain.co", true)]
    [InlineData("user_name@domain.co.uk", true)]
    public void UserPasswordChecking(string password, bool expectedResult)
    {
        var user = new User();

        user.IsPasswordValid(password).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(ERole.Manager, ERole.Supervisor)]
    [InlineData(ERole.Supervisor, ERole.Manager)]
    [InlineData(ERole.Owner, ERole.Supervisor)]
    [InlineData(ERole.Supervisor, ERole.Owner)]
    [InlineData(ERole.Researcher, ERole.Supervisor)]
    [InlineData(ERole.Supervisor, ERole.Researcher)]
    public void UserAssigningAndCheckingRole(ERole hasRole, ERole shouldBe)
    {
        var user = new User();

        user.AssignRole(hasRole);
        user.Should().NotBeNull();
        user.AssignRole(shouldBe);

        user.Role.Should().Be(shouldBe);
        user.HasRole(shouldBe).Should().BeTrue();
    }

    [Fact]
    public void GetPersonFullName()
    {
        User user = new User();
        user.FirstName = "John";
        user.LastName = "Smith";

        user.GetFullName().Should().BeOneOf(["John Smith", "Smith John"]);
    }

    [Theory]
    [InlineData("1234567890", "Ukraine", "Kyiv Region", "Kyiv District", "Kyiv", 50.4501, 30.5234)]
    [InlineData("+380501234567", "Ukraine", "Lviv Region", "Lviv District", "Lviv", 49.8397, 24.0297)]
    public void CheckChangingContactPersonDetails(string phoneNumber, string country, string region, string district, string city, double latitude, double longitude)
    {
        var user = new User();
        var location = new Location
        {
            Country = country,
            Region = region,
            District = district,
            City = city,
            Latitude = latitude,
            Longitude = longitude
        };

        user.UpdateContactDetails(phoneNumber, location);

        user.PhoneNumber.Should().Be(phoneNumber);
        user.Address.Should().BeEquivalentTo(location);
    }
}
