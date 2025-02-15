using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Responces;
using Avito_Backend_Trainee_Assignment_Winter_2025.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Authentication;

namespace Tests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var authRequest = new AuthRequest { Username = "test", Password = "password" };
        var authResponse = new AuthResponce { Token = "valid-token" };

        _mockAuthService.Setup(service => service.Authenticate(authRequest))
                        .ReturnsAsync(authResponse);

        // Act
        var result = await _controller.Authenticate(authRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponce>(okResult.Value);
        Assert.Equal("valid-token", response.Token);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var authRequest = new AuthRequest { Username = "test", Password = "wrongpassword" };

        _mockAuthService.Setup(service => service.Authenticate(authRequest))
                        .ThrowsAsync(new InvalidCredentialException());

        // Act
        var result = await _controller.Authenticate(authRequest);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var errorResponse = Assert.IsType<ErrorResponce>(unauthorizedResult.Value);
        Assert.Equal("Invalid credentials", errorResponse.Errors);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnServerError_WhenExceptionOccurs()
    {
        // Arrange
        var authRequest = new AuthRequest { Username = "test", Password = "password" };
        _mockAuthService.Setup(service => service.Authenticate(authRequest))
                        .ThrowsAsync(new Exception("Internal server error"));

        // Act
        var result = await _controller.Authenticate(authRequest);

        // Assert
        var serverErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, serverErrorResult.StatusCode);
        var errorResponse = Assert.IsType<ErrorResponce>(serverErrorResult.Value);
        Assert.Equal("Internal server error", errorResponse.Errors);
    }

}
