using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockService;
    private readonly AuthController     _controller;

    public AuthControllerTests()
    {
        _mockService = new Mock<IAuthService>();
        _controller  = new AuthController(_mockService.Object);
    }

    // =====================================================================
    // 1. LOGIN — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Login_ShouldReturn200_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email    = "admin@hospital.com",
            Password = "Admin@123"
        };

        var response = new LoginResponseDto
        {
            Token     = "jwt-token-here",
            Username  = "admin",
            Role      = "admin",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        _mockService
            .Setup(s => s.LoginAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(response);
    }

    // =====================================================================
    // 2. LOGIN — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Login_ShouldThrowUnauthorizedException_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email    = "wrong@hospital.com",
            Password = "WrongPass"
        };

        _mockService
            .Setup(s => s.LoginAsync(request))
            .ThrowsAsync(new UnauthorizedException("Invalid email or password."));

        // Act
        var action = async () => await _controller.Login(request);

        // Assert
        await action.Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("Invalid email or password.");
    }

    // =====================================================================
    // 3. GET USER BY ID — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetUserById_ShouldReturn200_WhenUserExists()
    {
        // Arrange
        var userId = 1;

        var user = new UserDto
        {
            UserId    = userId,
            Username  = "john_doe",
            Email     = "john@hospital.com",
            Role      = "physician",
            IsActive  = true,
            CreatedAt = DateTime.UtcNow
        };

        _mockService
            .Setup(s => s.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserById(userId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(user);
    }

    // =====================================================================
    // 4. GET USER BY ID — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetUserById_ShouldThrowBadRequestException_WhenIdIsZeroOrNegative()
    {
        // Arrange — id = 0 triggers the guard in the controller before
        // the service is ever called
        var invalidId = 0;

        // Act
        var action = async () => await _controller.GetUserById(invalidId);

        // Assert
        await action.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("User ID must be a positive number.");

        // Service should never have been called
        _mockService.Verify(s => s.GetUserByIdAsync(It.IsAny<int>()), Times.Never);
    }

    // =====================================================================
    // 5. CREATE USER — POSITIVE
    // =====================================================================

    [Fact]
    public async Task CreateUser_ShouldReturn201_WhenUserCreatedSuccessfully()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Username = "nurse_jane",
            Email    = "jane@hospital.com",
            Password = "Secure@123",
            Role     = "nurse",
            RefId    = 10
        };

        var created = new UserDto
        {
            UserId    = 5,
            Username  = dto.Username,
            Email     = dto.Email,
            Role      = dto.Role,
            IsActive  = true,
            CreatedAt = DateTime.UtcNow
        };

        _mockService
            .Setup(s => s.CreateUserAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.CreateUser(dto);

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(201);
        objectResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 6. CREATE USER — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task CreateUser_ShouldThrowConflictException_WhenEmailAlreadyRegistered()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Username = "duplicate_user",
            Email    = "existing@hospital.com",
            Password = "Pass@123",
            Role     = "nurse"
        };

        _mockService
            .Setup(s => s.CreateUserAsync(dto))
            .ThrowsAsync(new ConflictException("Email is already registered."));

        // Act
        var action = async () => await _controller.CreateUser(dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage("Email is already registered.");
    }

    // =====================================================================
    // 7. CHANGE PASSWORD — POSITIVE
    // =====================================================================

    [Fact]
    public async Task ChangePassword_ShouldReturn200_WhenPasswordChangedSuccessfully()
    {
        // Arrange
        var userId = 3;
        var dto    = new ChangePasswordDto
        {
            CurrentPassword = "OldPass@123",
            NewPassword     = "NewPass@123"
        };

        _mockService
            .Setup(s => s.ChangePasswordAsync(userId, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ChangePassword(userId, dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 8. CHANGE PASSWORD — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task ChangePassword_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 999;
        var dto    = new ChangePasswordDto
        {
            CurrentPassword = "OldPass@123",
            NewPassword     = "NewPass@123"
        };

        _mockService
            .Setup(s => s.ChangePasswordAsync(userId, dto))
            .ThrowsAsync(new NotFoundException($"User with ID {userId} was not found."));

        // Act
        var action = async () => await _controller.ChangePassword(userId, dto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with ID {userId} was not found.");
    }
}
