using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

/// <remarks>
/// RoomController does NOT catch exceptions internally.  Guard checks
/// (id &lt;= 0 / roomNumber &lt;= 0) are performed in the controller before
/// the service is invoked.
/// </remarks>
public class RoomControllerTests
{
    private readonly Mock<IRoomService> _mockService;
    private readonly RoomController     _controller;

    public RoomControllerTests()
    {
        _mockService = new Mock<IRoomService>();
        _controller  = new RoomController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET BY ID — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldReturn200_WhenRoomExists()
    {
        // Arrange
        var roomNumber = 101;

        var room = new RoomDto
        {
            RoomNumber  = roomNumber,
            RoomType    = "Single",
            BlockFloor  = 1,
            BlockCode   = 1,
            Unavailable = false
        };

        _mockService
            .Setup(s => s.GetByIdAsync(roomNumber))
            .ReturnsAsync(room);

        // Act
        var result = await _controller.GetById(roomNumber);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(room);
    }

    // =====================================================================
    // 2. GET BY ID — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldThrowBadRequestException_WhenRoomNumberIsNonPositive()
    {
        // Arrange — controller guard fires before service is called
        var invalidId = 0;

        // Act
        var action = async () => await _controller.GetById(invalidId);

        // Assert
        await action.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Room number must be a positive integer.");

        _mockService.Verify(s => s.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    // =====================================================================
    // 3. CREATE ROOM — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldReturn201_WhenRoomCreatedSuccessfully()
    {
        // Arrange
        var roomNumber = 205;
        var dto        = new RoomWriteDto
        {
            RoomType   = "Double",
            BlockFloor = 2,
            BlockCode  = 3
        };

        var created = new RoomDto
        {
            RoomNumber  = roomNumber,
            RoomType    = dto.RoomType,
            BlockFloor  = dto.BlockFloor,
            BlockCode   = dto.BlockCode,
            Unavailable = false
        };

        _mockService
            .Setup(s => s.CreateAsync(roomNumber, dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.Create(roomNumber, dto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. CREATE ROOM — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldThrowConflictException_WhenRoomAlreadyExists()
    {
        // Arrange
        var roomNumber = 101;   // already exists
        var dto        = new RoomWriteDto
        {
            RoomType   = "Single",
            BlockFloor = 1,
            BlockCode  = 1
        };

        _mockService
            .Setup(s => s.CreateAsync(roomNumber, dto))
            .ThrowsAsync(new ConflictException($"Room {roomNumber} already exists."));

        // Act
        var action = async () => await _controller.Create(roomNumber, dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage($"Room {roomNumber} already exists.");
    }

    // =====================================================================
    // 5. UPDATE ROOM — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldReturn200_WhenRoomUpdatedSuccessfully()
    {
        // Arrange
        var roomNumber = 101;
        var dto        = new RoomWriteDto
        {
            RoomType   = "Suite",
            BlockFloor = 1,
            BlockCode  = 1
        };

        var updated = new RoomDto
        {
            RoomNumber  = roomNumber,
            RoomType    = dto.RoomType,
            BlockFloor  = dto.BlockFloor,
            BlockCode   = dto.BlockCode,
            Unavailable = false
        };

        _mockService
            .Setup(s => s.UpdateAsync(roomNumber, dto))
            .ReturnsAsync(updated);

        // Act
        var result = await _controller.Update(roomNumber, dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(updated);
    }

    // =====================================================================
    // 6. UPDATE ROOM — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomNumber = 999;
        var dto        = new RoomWriteDto { RoomType = "Suite", BlockFloor = 9, BlockCode = 9 };

        _mockService
            .Setup(s => s.UpdateAsync(roomNumber, dto))
            .ThrowsAsync(new NotFoundException($"Room {roomNumber} not found."));

        // Act
        var action = async () => await _controller.Update(roomNumber, dto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Room {roomNumber} not found.");
    }

    // =====================================================================
    // 7. UPDATE AVAILABILITY — POSITIVE
    // =====================================================================

    [Fact]
    public async Task UpdateAvailability_ShouldReturn200_WhenRoomMarkedUnavailable()
    {
        // Arrange
        var roomNumber = 101;

        _mockService
            .Setup(s => s.UpdateAvailabilityAsync(roomNumber, true))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateAvailability(roomNumber, unavailable: true);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 8. UPDATE AVAILABILITY — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task UpdateAvailability_ShouldThrowNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomNumber = 999;

        _mockService
            .Setup(s => s.UpdateAvailabilityAsync(roomNumber, It.IsAny<bool>()))
            .ThrowsAsync(new NotFoundException($"Room {roomNumber} not found."));

        // Act
        var action = async () => await _controller.UpdateAvailability(roomNumber, unavailable: true);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Room {roomNumber} not found.");
    }
}
