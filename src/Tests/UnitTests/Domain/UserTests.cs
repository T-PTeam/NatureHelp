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
    [InlineData("password", false)]
    [InlineData("Password", false)]
    [InlineData("Password1", false)]
    [InlineData("Pass1@", false)]
    [InlineData("Password1@", true)]
    [InlineData("P@ssw0rd", true)]
    [InlineData("MyPass123!", true)]
    [InlineData("mypassword1@", false)]
    [InlineData("MYPASSWORD1@", false)]
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
    [InlineData("1234567890")]
    [InlineData("+380501234567")]
    public void CheckChangingContactPersonDetails(string phoneNumber)
    {
        var user = new User();

        user.UpdateContactDetails(phoneNumber);

        user.PhoneNumber.Should().Be(phoneNumber);
        user.PhoneNumber.Should().BeEquivalentTo(phoneNumber);
    }
}
