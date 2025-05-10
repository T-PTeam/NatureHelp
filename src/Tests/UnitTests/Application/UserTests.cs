using Application.Dtos;
using Application.Interfaces.Services.Organization;
using Application.Services.Organization;
using Domain.Models.Organization;
using FluentAssertions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.UnitTests.Application;
public class UserTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<PasswordHasher<User>> _passwordHasherMock = new();
    private readonly IUserService _userService;

    public UserTests()
    {
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
    {
        _userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var act = () => _userService.LoginAsync(new UserLoginDto { Email = "test@example.com", Password = "password123" });

        await act.Should().ThrowAsync<NullReferenceException>()
            .WithMessage("User was not found.");
    }

    [Fact]
    public async void LoginAsync_Should_Throw_VerificationFailed()
    {
        UserLoginDto userLoginDto = new UserLoginDto()
        {
            Email = "test@example.com",
            Password = "10101010"
        };
        User user = new User() { 
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "somepasswordHash"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(userLoginDto.Email))
            .ReturnsAsync(user);

        _passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, "wrongpasswordHash"))
            .Returns(PasswordVerificationResult.Failed);

        var act = () => _userService.LoginAsync(userLoginDto);

        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Password verification failed");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUser_WhenPasswordIsCorrect()
    {
        var user = new User { Email = "test@example.com", PasswordHash = "hashedpassword" };

        _userRepositoryMock.Setup(x => x.GetUserByEmail(user.Email))
            .ReturnsAsync(user);

        _passwordHasherMock.Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, "password123"))
            .Returns(PasswordVerificationResult.Success);

        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(user));

        var result = await _userService.LoginAsync(new UserLoginDto { Email = user.Email, Password = "password123", PasswordHash = user.PasswordHash });

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.AccessTokenExpireTime.Should().BeAfter(DateTime.UtcNow);
        result.RefreshTokenExpireTime.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task LoginAsync_ShouldSucceed_WhenPasswordHashMatches()
    {
        var user = new User { Email = "test@example.com", PasswordHash = "correcthash" };

        _userRepositoryMock.Setup(x => x.GetUserByEmail(user.Email))
            .ReturnsAsync(user);

        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(user));

        var result = await _userService.LoginAsync(new UserLoginDto
        {
            Email = user.Email,
            PasswordHash = "correcthash"
        });

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }
}
