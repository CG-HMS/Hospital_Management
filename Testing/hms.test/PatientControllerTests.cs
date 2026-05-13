using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs.Patient;
using Hms.API.Exceptions;
using Hms.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

/// <remarks>
/// PatientController does NOT catch exceptions — they propagate to the
/// global ExceptionMiddleware.  Negative tests assert on the thrown type.
/// </remarks>
public class PatientControllerTests
{
    private readonly Mock<IPatientService> _mockService;
    private readonly PatientController     _controller;

    public PatientControllerTests()
    {
        _mockService = new Mock<IPatientService>();
        _controller  = new PatientController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET PATIENT BY SSN — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetPatientById_ShouldReturn200_WhenPatientExists()
    {
        // Arrange
        var ssn = 100001;

        var patient = new PatientResponseDto
        {
            Ssn         = ssn,
            Name        = "Robert Miles",
            Address     = "123 Main St",
            Phone       = "555-1234",
            InsuranceId = 9,
            Pcp         = 3
        };

        _mockService
            .Setup(s => s.GetPatientByIdAsync(ssn))
            .ReturnsAsync(patient);

        // Act
        var result = await _controller.GetPatientById(ssn);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(patient);
    }

    // =====================================================================
    // 2. GET PATIENT BY SSN — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetPatientById_ShouldThrowNotFoundException_WhenPatientDoesNotExist()
    {
        // Arrange
        var ssn = 999999;

        _mockService
            .Setup(s => s.GetPatientByIdAsync(ssn))
            .ThrowsAsync(new NotFoundException($"Patient with SSN {ssn} not found."));

        // Act
        var action = async () => await _controller.GetPatientById(ssn);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Patient with SSN {ssn} not found.");
    }

    // =====================================================================
    // 3. CREATE PATIENT — POSITIVE
    // =====================================================================

    [Fact]
    public async Task CreatePatient_ShouldReturn201_WhenPatientCreatedSuccessfully()
    {
        // Arrange
        var dto = new PatientRequestDto
        {
            Ssn         = 200001,
            Name        = "Emily Clark",
            Address     = "456 Oak Ave",
            Phone       = "555-5678",
            InsuranceId = 7,
            Pcp         = 2
        };

        var created = new PatientResponseDto
        {
            Ssn         = dto.Ssn,
            Name        = dto.Name,
            Address     = dto.Address,
            Phone       = dto.Phone,
            InsuranceId = dto.InsuranceId,
            Pcp         = dto.Pcp
        };

        _mockService
            .Setup(s => s.CreatePatientAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.CreatePatient(dto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. CREATE PATIENT — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task CreatePatient_ShouldThrowConflictException_WhenPatientAlreadyExists()
    {
        // Arrange
        var dto = new PatientRequestDto
        {
            Ssn         = 100001,   // already registered
            Name        = "Robert Miles",
            Address     = "123 Main St",
            Phone       = "555-1234",
            InsuranceId = 9,
            Pcp         = 3
        };

        _mockService
            .Setup(s => s.CreatePatientAsync(dto))
            .ThrowsAsync(new ConflictException($"Patient with SSN {dto.Ssn} already exists."));

        // Act
        var action = async () => await _controller.CreatePatient(dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage($"Patient with SSN {dto.Ssn} already exists.");
    }

    // =====================================================================
    // 5. UPDATE PATIENT — POSITIVE
    // =====================================================================

    [Fact]
    public async Task UpdatePatient_ShouldReturn200_WhenPatientUpdatedSuccessfully()
    {
        // Arrange
        var ssn = 100001;
        var dto = new PatientRequestDto
        {
            Ssn         = ssn,
            Name        = "Robert Miles Jr.",
            Address     = "789 Pine Rd",
            Phone       = "555-9999",
            InsuranceId = 9,
            Pcp         = 3
        };

        _mockService
            .Setup(s => s.UpdatePatientAsync(ssn, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdatePatient(ssn, dto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 6. UPDATE PATIENT — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task UpdatePatient_ShouldThrowNotFoundException_WhenPatientDoesNotExist()
    {
        // Arrange
        var ssn = 999999;
        var dto = new PatientRequestDto
        {
            Ssn         = ssn,
            Name        = "Ghost Patient",
            Address     = "N/A",
            Phone       = "000-0000",
            InsuranceId = 0,
            Pcp         = 0
        };

        _mockService
            .Setup(s => s.UpdatePatientAsync(ssn, dto))
            .ThrowsAsync(new NotFoundException($"Patient with SSN {ssn} not found."));

        // Act
        var action = async () => await _controller.UpdatePatient(ssn, dto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Patient with SSN {ssn} not found.");
    }

    // =====================================================================
    // 7. DELETE PATIENT — POSITIVE
    // =====================================================================

    [Fact]
    public async Task DeletePatient_ShouldReturn200_WhenPatientDeletedSuccessfully()
    {
        // Arrange
        var ssn = 100001;

        _mockService
            .Setup(s => s.DeletePatientAsync(ssn))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePatient(ssn);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // =====================================================================
    // 8. DELETE PATIENT — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task DeletePatient_ShouldThrowNotFoundException_WhenPatientDoesNotExist()
    {
        // Arrange
        var ssn = 999999;

        _mockService
            .Setup(s => s.DeletePatientAsync(ssn))
            .ThrowsAsync(new NotFoundException($"Patient with SSN {ssn} not found."));

        // Act
        var action = async () => await _controller.DeletePatient(ssn);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Patient with SSN {ssn} not found.");
    }
}
