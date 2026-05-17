using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

/// <remarks>
/// StayController does NOT catch exceptions internally.  Two actions
/// (GetById, Update) call StayExistsAsync / check for null and throw
/// NotFoundException directly from the controller.  GetByDateRange
/// validates the date range before calling the service.
/// </remarks>
public class StayControllerTests
{
    private readonly Mock<IStayService> _mockService;
    private readonly StayController     _controller;

    public StayControllerTests()
    {
        _mockService = new Mock<IStayService>();
        _controller  = new StayController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET BY ID — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldReturn200_WhenStayExists()
    {
        // Arrange
        var stayId = 1;

        var stay = new StayDetailDTO
        {
            StayId      = stayId,
            Patient     = 101,
            Room        = 205,
            StayStart   = DateTime.UtcNow.AddDays(-3),
            StayEnd     = DateTime.UtcNow.AddDays(2),
            PatientName = "Alice Cooper",
            RoomNumber  = "205",
            DaysOfStay  = 5
        };

        _mockService
            .Setup(s => s.GetStayByIdAsync(stayId))
            .ReturnsAsync(stay);

        // Act
        var result = await _controller.GetById(stayId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(stay);
    }

    // =====================================================================
    // 2. GET BY ID — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldThrowNotFoundException_WhenStayDoesNotExist()
    {
        // Arrange — service returns null, controller throws NotFoundException
        var stayId = 999;

        _mockService
            .Setup(s => s.GetStayByIdAsync(stayId))
            .ReturnsAsync((StayDetailDTO?)null);

        // Act
        var action = async () => await _controller.GetById(stayId);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*Stay*{stayId}*");
    }

    // =====================================================================
    // 3. CREATE STAY — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldReturn201_WhenStayCreatedSuccessfully()
    {
        // Arrange
        var dto = new CreateStayDTO
        {
            Patient   = 101,
            Room      = 205,
            StayStart = DateTime.UtcNow,
            StayEnd   = DateTime.UtcNow.AddDays(5)
        };

        var created = new StayDTO
        {
            StayId    = 10,
            Patient   = dto.Patient,
            Room      = dto.Room,
            StayStart = dto.StayStart,
            StayEnd   = dto.StayEnd
        };

        _mockService
            .Setup(s => s.CreateStayAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. CREATE STAY — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldThrowConflictException_WhenPatientAlreadyHasActiveStay()
    {
        // Arrange
        var dto = new CreateStayDTO
        {
            Patient   = 101,
            Room      = 205,
            StayStart = DateTime.UtcNow,
            StayEnd   = DateTime.UtcNow.AddDays(3)
        };

        _mockService
            .Setup(s => s.CreateStayAsync(dto))
            .ThrowsAsync(new ConflictException("Patient already has an active stay."));

        // Act
        var action = async () => await _controller.Create(dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage("Patient already has an active stay.");
    }

    // =====================================================================
    // 5. UPDATE STAY — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldReturn200_WhenStayUpdatedSuccessfully()
    {
        // Arrange
        var stayId    = 1;
        var updateDto = new UpdateStayDTO
        {
            Patient   = 101,
            Room      = 210,
            StayStart = DateTime.UtcNow.AddDays(-3),
            StayEnd   = DateTime.UtcNow.AddDays(4)
        };

        var updated = new StayDTO
        {
            StayId    = stayId,
            Patient   = updateDto.Patient,
            Room      = updateDto.Room,
            StayStart = updateDto.StayStart,
            StayEnd   = updateDto.StayEnd
        };

        _mockService
            .Setup(s => s.StayExistsAsync(stayId))
            .ReturnsAsync(true);

        _mockService
            .Setup(s => s.UpdateStayAsync(stayId, updateDto))
            .ReturnsAsync(updated);

        // Act
        var result = await _controller.Update(stayId, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(updated);
    }

    // =====================================================================
    // 6. UPDATE STAY — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldThrowNotFoundException_WhenStayDoesNotExist()
    {
        // Arrange — StayExistsAsync returns false, controller throws
        var stayId    = 999;
        var updateDto = new UpdateStayDTO
        {
            Patient   = 101,
            Room      = 210,
            StayStart = DateTime.UtcNow,
            StayEnd   = DateTime.UtcNow.AddDays(2)
        };

        _mockService
            .Setup(s => s.StayExistsAsync(stayId))
            .ReturnsAsync(false);

        // Act
        var action = async () => await _controller.Update(stayId, updateDto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*Stay*{stayId}*");

        _mockService.Verify(s => s.UpdateStayAsync(It.IsAny<int>(), It.IsAny<UpdateStayDTO>()), Times.Never);
    }

    // =====================================================================
    // 7. GET BY DATE RANGE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetByDateRange_ShouldReturn200_WhenDateRangeIsValid()
    {
        // Arrange
        var start = DateTime.UtcNow.AddDays(-14);
        var end   = DateTime.UtcNow;

        var stays = new List<StayDTO>
        {
            new() { StayId = 1, Patient = 101, Room = 205, StayStart = start.AddDays(1), StayEnd = end }
        };

        _mockService
            .Setup(s => s.GetStaysByDateRangeAsync(start, end))
            .ReturnsAsync(stays);

        // Act
        var result = await _controller.GetByDateRange(start, end);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 8. GET BY DATE RANGE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetByDateRange_ShouldThrowBadRequestException_WhenStartIsAfterEnd()
    {
        // Arrange — controller validates date order before calling service
        var start = DateTime.UtcNow;
        var end   = DateTime.UtcNow.AddDays(-14);   // end is BEFORE start

        // Act
        var action = async () => await _controller.GetByDateRange(start, end);

        // Assert
        await action.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Start date must be earlier than end date");

        _mockService.Verify(s => s.GetStaysByDateRangeAsync(
            It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
    }
}
