using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs.Medication;
using Hms.API.Exceptions;
using Hms.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

/// <remarks>
/// MedicationController does NOT catch exceptions internally — they bubble
/// up to the global ExceptionMiddleware.  Negative tests therefore assert
/// on the thrown exception rather than on an HTTP status code.
/// </remarks>
public class MedicationControllerTests
{
    private readonly Mock<IMedicationService> _mockService;
    private readonly MedicationController     _controller;

    public MedicationControllerTests()
    {
        _mockService = new Mock<IMedicationService>();
        _controller  = new MedicationController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET MEDICATION BY ID — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetMedicationById_ShouldReturn200_WhenMedicationExists()
    {
        // Arrange
        var code = 101;

        var medication = new MedicationResponseDto
        {
            Code        = code,
            Name        = "Paracetamol",
            Brand       = "Calpol",
            Description = "Analgesic and antipyretic"
        };

        _mockService
            .Setup(s => s.GetMedicationByIdAsync(code))
            .ReturnsAsync(medication);

        // Act
        var result = await _controller.GetMedicationById(code);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(medication);
    }

    // =====================================================================
    // 2. GET MEDICATION BY ID — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetMedicationById_ShouldThrowNotFoundException_WhenMedicationDoesNotExist()
    {
        // Arrange
        var code = 999;

        _mockService
            .Setup(s => s.GetMedicationByIdAsync(code))
            .ThrowsAsync(new NotFoundException($"Medication with code {code} not found."));

        // Act
        var action = async () => await _controller.GetMedicationById(code);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Medication with code {code} not found.");
    }

    // =====================================================================
    // 3. CREATE MEDICATION — POSITIVE
    // =====================================================================

    [Fact]
    public async Task CreateMedication_ShouldReturn201_WhenMedicationCreatedSuccessfully()
    {
        // Arrange
        var dto = new MedicationRequestDto
        {
            Code        = 201,
            Name        = "Ibuprofen",
            Brand       = "Advil",
            Description = "Anti-inflammatory"
        };

        var created = new MedicationResponseDto
        {
            Code        = dto.Code,
            Name        = dto.Name,
            Brand       = dto.Brand,
            Description = dto.Description
        };

        _mockService
            .Setup(s => s.CreateMedicationAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.CreateMedication(dto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. CREATE MEDICATION — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task CreateMedication_ShouldThrowConflictException_WhenMedicationAlreadyExists()
    {
        // Arrange
        var dto = new MedicationRequestDto
        {
            Code        = 101,   // already in DB
            Name        = "Paracetamol",
            Brand       = "Calpol",
            Description = "Already registered"
        };

        _mockService
            .Setup(s => s.CreateMedicationAsync(dto))
            .ThrowsAsync(new ConflictException($"Medication with code {dto.Code} already exists."));

        // Act
        var action = async () => await _controller.CreateMedication(dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage($"Medication with code {dto.Code} already exists.");
    }

    // =====================================================================
    // 5. UPDATE MEDICATION — POSITIVE
    // =====================================================================

    [Fact]
    public async Task UpdateMedication_ShouldReturn200_WhenMedicationUpdatedSuccessfully()
    {
        // Arrange
        var code = 101;
        var dto  = new MedicationRequestDto
        {
            Code        = code,
            Name        = "Paracetamol Updated",
            Brand       = "Calpol Plus",
            Description = "Updated description"
        };

        _mockService
            .Setup(s => s.UpdateMedicationAsync(code, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateMedication(code, dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 6. UPDATE MEDICATION — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task UpdateMedication_ShouldThrowNotFoundException_WhenMedicationDoesNotExist()
    {
        // Arrange
        var code = 999;
        var dto  = new MedicationRequestDto
        {
            Code        = code,
            Name        = "Ghost Drug",
            Brand       = "N/A",
            Description = "Does not exist"
        };

        _mockService
            .Setup(s => s.UpdateMedicationAsync(code, dto))
            .ThrowsAsync(new NotFoundException($"Medication with code {code} not found."));

        // Act
        var action = async () => await _controller.UpdateMedication(code, dto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Medication with code {code} not found.");
    }

    // =====================================================================
    // 7. DELETE MEDICATION — POSITIVE
    // =====================================================================

    [Fact]
    public async Task DeleteMedication_ShouldReturn200_WhenMedicationDeletedSuccessfully()
    {
        // Arrange
        var code = 101;

        _mockService
            .Setup(s => s.DeleteMedicationAsync(code))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteMedication(code);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 8. DELETE MEDICATION — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task DeleteMedication_ShouldThrowNotFoundException_WhenMedicationDoesNotExist()
    {
        // Arrange
        var code = 999;

        _mockService
            .Setup(s => s.DeleteMedicationAsync(code))
            .ThrowsAsync(new NotFoundException($"Medication with code {code} not found."));

        // Act
        var action = async () => await _controller.DeleteMedication(code);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Medication with code {code} not found.");
    }
}
