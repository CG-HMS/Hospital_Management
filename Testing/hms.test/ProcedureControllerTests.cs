using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

/// <remarks>
/// ProcedureController does NOT catch exceptions internally.  Guard checks
/// (code &lt;= 0) are performed in the controller before calling the service.
/// </remarks>
public class ProcedureControllerTests
{
    private readonly Mock<IProcedureService> _mockService;
    private readonly ProcedureController     _controller;

    public ProcedureControllerTests()
    {
        _mockService = new Mock<IProcedureService>();
        _controller  = new ProcedureController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET PROCEDURE BY CODE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetProcedure_ShouldReturn200_WhenProcedureExists()
    {
        // Arrange
        var code = 1001;

        var procedure = new ProcedureDto
        {
            Code = code,
            Name = "Appendectomy",
            Cost = 5000m
        };

        _mockService
            .Setup(s => s.GetProcedureByCode(code))
            .ReturnsAsync(procedure);

        // Act
        var result = await _controller.GetProcedure(code);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(procedure);
    }

    // =====================================================================
    // 2. GET PROCEDURE BY CODE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetProcedure_ShouldThrowBadRequestException_WhenCodeIsNonPositive()
    {
        // Arrange — controller guard fires before service is called
        var invalidCode = 0;

        // Act
        var action = async () => await _controller.GetProcedure(invalidCode);

        // Assert
        await action.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Procedure code must be positive.");

        _mockService.Verify(s => s.GetProcedureByCode(It.IsAny<int>()), Times.Never);
    }

    // =====================================================================
    // 3. ADD PROCEDURE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task AddProcedure_ShouldReturn200_WhenProcedureCreatedSuccessfully()
    {
        // Arrange
        var dto = new ProcedureWriteDto
        {
            Name = "Colonoscopy",
            Cost = 1200m
        };

        var created = new ProcedureDto
        {
            Code = 2001,
            Name = dto.Name!,
            Cost = dto.Cost!.Value
        };

        _mockService
            .Setup(s => s.AddProcedure(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.AddProcedure(dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. ADD PROCEDURE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task AddProcedure_ShouldThrowConflictException_WhenProcedureAlreadyExists()
    {
        // Arrange
        var dto = new ProcedureWriteDto
        {
            Name = "Appendectomy",
            Cost = 5000m
        };

        _mockService
            .Setup(s => s.AddProcedure(dto))
            .ThrowsAsync(new ConflictException("A procedure with this name already exists."));

        // Act
        var action = async () => await _controller.AddProcedure(dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage("A procedure with this name already exists.");
    }

    // =====================================================================
    // 5. UPDATE PROCEDURE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task UpdateProcedure_ShouldReturn200_WhenProcedureUpdatedSuccessfully()
    {
        // Arrange
        var code = 1001;
        var dto  = new ProcedureWriteDto
        {
            Name = "Appendectomy (Revised)",
            Cost = 5500m
        };

        var updated = new ProcedureDto
        {
            Code = code,
            Name = dto.Name!,
            Cost = dto.Cost!.Value
        };

        _mockService
            .Setup(s => s.UpdateProcedure(code, dto))
            .ReturnsAsync(updated);

        // Act
        var result = await _controller.UpdateProcedure(code, dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(updated);
    }

    // =====================================================================
    // 6. UPDATE PROCEDURE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task UpdateProcedure_ShouldThrowNotFoundException_WhenProcedureDoesNotExist()
    {
        // Arrange
        var code = 9999;
        var dto  = new ProcedureWriteDto { Name = "Ghost Op", Cost = 0 };

        _mockService
            .Setup(s => s.UpdateProcedure(code, dto))
            .ThrowsAsync(new NotFoundException($"Procedure with code {code} not found."));

        // Act
        var action = async () => await _controller.UpdateProcedure(code, dto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Procedure with code {code} not found.");
    }

    // =====================================================================
    // 7. DELETE PROCEDURE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task DeleteProcedure_ShouldReturn200_WhenProcedureDeletedSuccessfully()
    {
        // Arrange
        var code = 1001;

        _mockService
            .Setup(s => s.DeleteProcedure(code))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProcedure(code);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 8. DELETE PROCEDURE — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task DeleteProcedure_ShouldThrowNotFoundException_WhenProcedureDoesNotExist()
    {
        // Arrange
        var code = 9999;

        _mockService
            .Setup(s => s.DeleteProcedure(code))
            .ThrowsAsync(new NotFoundException($"Procedure with code {code} not found."));

        // Act
        var action = async () => await _controller.DeleteProcedure(code);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Procedure with code {code} not found.");
    }
}
