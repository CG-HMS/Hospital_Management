using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

/// <remarks>
/// PhysicianController does NOT catch exceptions — they propagate to the
/// global ExceptionMiddleware.  Guard checks (id &lt;= 0) throw directly
/// from the controller without calling the service.
/// </remarks>
public class PhysicianControllerTests
{
    private readonly Mock<IPhysicianService> _mockService;
    private readonly PhysicianController     _controller;

    public PhysicianControllerTests()
    {
        _mockService = new Mock<IPhysicianService>();
        _controller  = new PhysicianController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET PHYSICIAN BY ID — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetPhysicianById_ShouldReturn200_WhenPhysicianExists()
    {
        // Arrange
        var id = 1;

        var physician = new PhysicianDto
        {
            EmployeeId = id,
            Name       = "Dr. Gregory House",
            Position   = "Diagnostician"
        };

        _mockService
            .Setup(s => s.GetPhysicianById(id))
            .ReturnsAsync(physician);

        // Act
        var result = await _controller.GetPhysicianById(id);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(physician);
    }

    // =====================================================================
    // 2. GET PHYSICIAN BY ID — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetPhysicianById_ShouldThrowBadRequestException_WhenIdIsInvalid()
    {
        // Arrange — controller guards against id <= 0 before calling service
        var invalidId = -1;

        // Act
        var action = async () => await _controller.GetPhysicianById(invalidId);

        // Assert
        await action.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Physician ID must be positive.");

        _mockService.Verify(s => s.GetPhysicianById(It.IsAny<int>()), Times.Never);
    }

    // =====================================================================
    // 3. ADD PHYSICIAN — POSITIVE
    // =====================================================================

    [Fact]
    public async Task AddPhysician_ShouldReturn200_WhenPhysicianCreatedSuccessfully()
    {
        // Arrange
        var dto = new PhysicianWriteDto
        {
            Name     = "Dr. Lisa Cuddy",
            Position = "Dean of Medicine",
            Ssn      = 123456
        };

        var created = new PhysicianDto
        {
            EmployeeId = 10,
            Name       = dto.Name!,
            Position   = dto.Position!
        };

        _mockService
            .Setup(s => s.AddPhysician(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.AddPhysician(dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. ADD PHYSICIAN — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task AddPhysician_ShouldThrowConflictException_WhenPhysicianAlreadyExists()
    {
        // Arrange
        var dto = new PhysicianWriteDto
        {
            Name     = "Dr. Gregory House",
            Position = "Diagnostician",
            Ssn      = 999999
        };

        _mockService
            .Setup(s => s.AddPhysician(dto))
            .ThrowsAsync(new ConflictException("A physician with this SSN already exists."));

        // Act
        var action = async () => await _controller.AddPhysician(dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage("A physician with this SSN already exists.");
    }

    // =====================================================================
    // 5. UPDATE PHYSICIAN — POSITIVE
    // =====================================================================

    [Fact]
    public async Task UpdatePhysician_ShouldReturn200_WhenPhysicianUpdatedSuccessfully()
    {
        // Arrange
        var id  = 1;
        var dto = new PhysicianWriteDto
        {
            Name     = "Dr. Gregory House Sr.",
            Position = "Chief of Diagnostics"
        };

        var updated = new PhysicianDto
        {
            EmployeeId = id,
            Name       = dto.Name!,
            Position   = dto.Position!
        };

        _mockService
            .Setup(s => s.UpdatePhysician(id, dto))
            .ReturnsAsync(updated);

        // Act
        var result = await _controller.UpdatePhysician(id, dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(updated);
    }

    // =====================================================================
    // 6. UPDATE PHYSICIAN — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task UpdatePhysician_ShouldThrowNotFoundException_WhenPhysicianDoesNotExist()
    {
        // Arrange
        var id  = 999;
        var dto = new PhysicianWriteDto { Name = "Ghost Doctor", Position = "N/A" };

        _mockService
            .Setup(s => s.UpdatePhysician(id, dto))
            .ThrowsAsync(new NotFoundException($"Physician with ID {id} not found."));

        // Act
        var action = async () => await _controller.UpdatePhysician(id, dto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Physician with ID {id} not found.");
    }

    // =====================================================================
    // 7. DELETE PHYSICIAN — POSITIVE
    // =====================================================================

    [Fact]
    public async Task DeletePhysician_ShouldReturn200_WhenPhysicianDeletedSuccessfully()
    {
        // Arrange
        var id = 1;

        _mockService
            .Setup(s => s.DeletePhysician(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePhysician(id);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 8. DELETE PHYSICIAN — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task DeletePhysician_ShouldThrowNotFoundException_WhenPhysicianDoesNotExist()
    {
        // Arrange
        var id = 999;

        _mockService
            .Setup(s => s.DeletePhysician(id))
            .ThrowsAsync(new NotFoundException($"Physician with ID {id} not found."));

        // Act
        var action = async () => await _controller.DeletePhysician(id);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Physician with ID {id} not found.");
    }
}
