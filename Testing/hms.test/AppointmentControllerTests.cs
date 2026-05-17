using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;
public class AppointmentControllerTests
{
    private readonly Mock<IAppointmentService> _mockService;
    private readonly AppointmentController     _controller;

    public AppointmentControllerTests()
    {
        _mockService = new Mock<IAppointmentService>();
        _controller  = new AppointmentController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET BY ID — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldReturn200_WhenAppointmentExists()
    {
        // Arrange
        var id = 1;

        var appointment = new AppointmentDto
        {
            AppointmentId   = id,
            Patient         = 101,
            Physician       = 5,
            Starto          = DateTime.UtcNow,
            Endo            = DateTime.UtcNow.AddHours(1),
            ExaminationRoom = "Room A"
        };

        _mockService
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(appointment);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(appointment);
    }

    // =====================================================================
    // 2. GET BY ID — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldReturn404_WhenAppointmentDoesNotExist()
    {
        // Arrange
        var id = 999;

        _mockService
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((AppointmentDto?)null);

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
    public async Task Create_ShouldReturn201_WhenAppointmentCreatedSuccessfully()
    {
        // Arrange
        var dto = new AppointmentCreateDto
        {
            Patient         = 101,
            Physician       = 5,
            Starto          = DateTime.UtcNow,
            Endo            = DateTime.UtcNow.AddHours(1),
            ExaminationRoom = "Room B"
        };

        var created = new AppointmentDto
        {
            AppointmentId   = 10,
            Patient         = dto.Patient,
            Physician       = dto.Physician,
            Starto          = dto.Starto,
            Endo            = dto.Endo,
            ExaminationRoom = dto.ExaminationRoom
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
        var dto = new AppointmentCreateDto
        {
            Patient         = 101,
            Physician       = 5,
            Starto          = DateTime.UtcNow.AddHours(2), // start AFTER end
            Endo            = DateTime.UtcNow,
            ExaminationRoom = "Room C"
        };

        _mockService
            .Setup(s => s.AddAsync(dto))
            .ThrowsAsync(new ValidationException("Start time must be before end time."));

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
    public async Task Update_ShouldReturn204_WhenAppointmentUpdatedSuccessfully()
    {
        // Arrange
        var id  = 1;
        var dto = new AppointmentUpdateDto
        {
            Patient         = 101,
            Physician       = 5,
            Starto          = DateTime.UtcNow,
            Endo            = DateTime.UtcNow.AddHours(2),
            ExaminationRoom = "Room A"
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
    public async Task Update_ShouldReturn404_WhenAppointmentDoesNotExist()
    {
        // Arrange
        var id  = 999;
        var dto = new AppointmentUpdateDto
        {
            Patient         = 101,
            Physician       = 5,
            Starto          = DateTime.UtcNow,
            Endo            = DateTime.UtcNow.AddHours(1),
            ExaminationRoom = "Room X"
        };

        _mockService
            .Setup(s => s.UpdateAsync(id, dto))
            .ThrowsAsync(new NotFoundException($"Appointment with ID {id} not found."));

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
    public async Task Delete_ShouldReturn204_WhenAppointmentDeletedSuccessfully()
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
    public async Task Delete_ShouldReturn404_WhenAppointmentDoesNotExist()
    {
        // Arrange
        var id = 999;

        _mockService
            .Setup(s => s.DeleteAsync(id))
            .ThrowsAsync(new NotFoundException($"Appointment with ID {id} not found."));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(404);
    }
}
