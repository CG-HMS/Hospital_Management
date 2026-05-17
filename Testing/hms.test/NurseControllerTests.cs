using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

public class NurseControllerTests
{
    private readonly Mock<INurseService> _mockService;
    private readonly NurseController     _controller;

    public NurseControllerTests()
    {
        _mockService = new Mock<INurseService>();
        _controller  = new NurseController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET BY ID — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldReturn200_WhenNurseExists()
    {
        // Arrange
        var id = 1;

        var nurse = new NurseDto
        {
            EmployeeId = id,
            Name       = "Jane Smith",
            Position   = "Head Nurse",
            Registered = true,
            Ssn        = 555001
        };

        _mockService
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(nurse);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(nurse);
    }

    // =====================================================================
    // 2. GET BY ID — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldReturn404_WhenNurseDoesNotExist()
    {
        // Arrange
        var id = 999;

        _mockService
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((NurseDto?)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(404);
    }

    // =====================================================================
    // 3. CREATE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldReturn201_WhenNurseCreatedSuccessfully()
    {
        // Arrange
        var dto = new NurseCreateDto
        {
            Name       = "Alice Brown",
            Position   = "Registered Nurse",
            Registered = true,
            Ssn        = 555002
        };

        var created = new NurseDto
        {
            EmployeeId = 2,
            Name       = dto.Name,
            Position   = dto.Position,
            Registered = dto.Registered,
            Ssn        = dto.Ssn
        };

        _mockService
            .Setup(s => s.AddAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. CREATE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldReturn400_WhenValidationFails()
    {
        // Arrange
        var dto = new NurseCreateDto
        {
            Name       = "",   // invalid — empty name
            Position   = "Nurse",
            Registered = false,
            Ssn        = 0
        };

        _mockService
            .Setup(s => s.AddAsync(dto))
            .ThrowsAsync(new ValidationException("Nurse name cannot be empty."));

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.StatusCode.Should().Be(400);
    }

    // =====================================================================
    // 5. UPDATE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldReturn204_WhenNurseUpdatedSuccessfully()
    {
        // Arrange
        var id  = 1;
        var dto = new NurseUpdateDto
        {
            Name       = "Jane Smith-Updated",
            Position   = "Senior Nurse",
            Registered = true
        };

        _mockService
            .Setup(s => s.UpdateAsync(id, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(id, dto);

        // Assert
        result.Should().BeOfType<NoContentResult>()
            .Which.StatusCode.Should().Be(204);
    }

    // =====================================================================
    // 6. UPDATE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldReturn404_WhenNurseDoesNotExist()
    {
        // Arrange
        var id  = 999;
        var dto = new NurseUpdateDto
        {
            Name       = "Ghost Nurse",
            Position   = "ICU",
            Registered = false
        };

        _mockService
            .Setup(s => s.UpdateAsync(id, dto))
            .ThrowsAsync(new NotFoundException($"Nurse with ID {id} not found."));

        // Act
        var result = await _controller.Update(id, dto);

        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(404);
    }

    // =====================================================================
    // 7. DELETE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Delete_ShouldReturn204_WhenNurseDeletedSuccessfully()
    {
        // Arrange
        var id = 1;

        _mockService
            .Setup(s => s.DeleteAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>()
            .Which.StatusCode.Should().Be(204);
    }

    // =====================================================================
    // 8. DELETE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Delete_ShouldReturn404_WhenNurseDoesNotExist()
    {
        // Arrange
        var id = 999;

        _mockService
            .Setup(s => s.DeleteAsync(id))
            .ThrowsAsync(new NotFoundException($"Nurse with ID {id} not found."));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(404);
    }
}
