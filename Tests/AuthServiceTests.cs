
using Application.Models.Requests;
using Domain.Entities;
using Infrastucture.Services;
using Moq;
using System.Linq.Expressions;
using System.Security.Authentication;

namespace Tests;

public class AuthServiceTests : BaseTest
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(MockEncrypt.Object, MockTokenService.Object, MockUserRepository.Object);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var authRequest = new AuthRequest { Username = "test", Password = "password" };
        var user = new User { Username = "test", PasswordHash = "hashedPassword", PasswordSalt = "salt" };
        var token = "valid-jwt-token";

        MockUserRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null, null, null, null))
                           .ReturnsAsync(new List<User> { user });

        MockEncrypt.Setup(e => e.HashPassword("password", user.PasswordSalt))
                    .Returns(user.PasswordHash);

        MockTokenService.Setup(ts => ts.GenerateToken(user))
                         .Returns(token);

        // Act
        var response = await _authService.Authenticate(authRequest);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(token, response.Token);
    }

    [Fact]
    public async Task Authenticate_ShouldThrowInvalidCredentialException_WhenPasswordIsInvalid()
    {
        // Arrange
        var authRequest = new AuthRequest { Username = "test", Password = "wrongPassword" };
        var user = new User { Username = "test", PasswordHash = "hashedPassword", PasswordSalt = "salt" };

        MockUserRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null, null, null, null))
                           .ReturnsAsync(new List<User> { user });

        MockEncrypt.Setup(e => e.HashPassword("wrongPassword", user.PasswordSalt))
                    .Returns("wrongHash");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialException>(() => _authService.Authenticate(authRequest));
    }

}