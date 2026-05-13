using FluentAssertions;
using Hms.API.Controllers;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hms.API.Tests.Controllers;

/// <remarks>
/// PrescriptionController does NOT catch exceptions internally.
/// Tests for GetByDateRange (negative) verify the in-controller guard
/// (startDate &gt; endDate) that throws BadRequestException directly.
/// </remarks>
public class PrescriptionControllerTests
{
    private readonly Mock<IPrescriptionService> _mockService;
    private readonly PrescriptionController     _controller;

    public PrescriptionControllerTests()
    {
        _mockService = new Mock<IPrescriptionService>();
        _controller  = new PrescriptionController(_mockService.Object);
    }

    // =====================================================================
    // 1. GET BY COMPOSITE KEY — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldReturn200_WhenPrescriptionExists()
    {
        // Arrange
        int physician = 1, patient = 101, medication = 201;

        var prescription = new PrescriptionDetailDTO
        {
            Physician      = physician,
            Patient        = patient,
            Medication     = medication,
            Date           = DateTime.UtcNow,
            Dose           = "500mg twice daily",
            PhysicianName  = "Dr. House",
            PatientName    = "John Doe",
            MedicationName = "Ibuprofen"
        };

        _mockService
            .Setup(s => s.GetPrescriptionByIdAsync(physician, patient, medication))
            .ReturnsAsync(prescription);

        // Act
        var result = await _controller.GetById(physician, patient, medication);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(prescription);
    }

    // =====================================================================
    // 2. GET BY COMPOSITE KEY — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task GetById_ShouldThrowNotFoundException_WhenPrescriptionDoesNotExist()
    {
        // Arrange — service returns null, controller throws NotFoundException
        int physician = 1, patient = 999, medication = 201;

        _mockService
            .Setup(s => s.GetPrescriptionByIdAsync(physician, patient, medication))
            .ReturnsAsync((PrescriptionDetailDTO?)null);

        // Act
        var action = async () => await _controller.GetById(physician, patient, medication);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*{physician}*{patient}*{medication}*");
    }

    // =====================================================================
    // 3. CREATE PRESCRIPTION — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldReturn201_WhenPrescriptionCreatedSuccessfully()
    {
        // Arrange
        var dto = new CreatePrescriptionDTO
        {
            Physician   = 1,
            Patient     = 101,
            Medication  = 201,
            Date        = DateTime.UtcNow,
            Dose        = "250mg daily",
            Appointment = 5
        };

        var created = new PrescriptionDTO
        {
            Physician   = dto.Physician,
            Patient     = dto.Patient,
            Medication  = dto.Medication,
            Date        = dto.Date,
            Dose        = dto.Dose,
            Appointment = dto.Appointment
        };

        _mockService
            .Setup(s => s.CreatePrescriptionAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    // =====================================================================
    // 4. CREATE PRESCRIPTION — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Create_ShouldThrowConflictException_WhenPrescriptionAlreadyExists()
    {
        // Arrange
        var dto = new CreatePrescriptionDTO
        {
            Physician  = 1,
            Patient    = 101,
            Medication = 201,
            Date       = DateTime.UtcNow,
            Dose       = "500mg daily"
        };

        _mockService
            .Setup(s => s.CreatePrescriptionAsync(dto))
            .ThrowsAsync(new ConflictException("Prescription already exists for this physician/patient/medication."));

        // Act
        var action = async () => await _controller.Create(dto);

        // Assert
        await action.Should()
            .ThrowAsync<ConflictException>()
            .WithMessage("Prescription already exists for this physician/patient/medication.");
    }

    // =====================================================================
    // 5. UPDATE PRESCRIPTION — POSITIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldReturn200_WhenPrescriptionUpdatedSuccessfully()
    {
        // Arrange
        int physician = 1, patient = 101, medication = 201;

        var updateDto = new UpdatePrescriptionDTO
        {
            Physician  = physician,
            Patient    = patient,
            Medication = medication,
            Date       = DateTime.UtcNow,
            Dose       = "1000mg once daily"
        };

        var updated = new PrescriptionDTO
        {
            Physician  = physician,
            Patient    = patient,
            Medication = medication,
            Date       = updateDto.Date,
            Dose       = updateDto.Dose
        };

        _mockService
            .Setup(s => s.PrescriptionExistsAsync(physician, patient, medication))
            .ReturnsAsync(true);

        _mockService
            .Setup(s => s.UpdatePrescriptionAsync(physician, patient, medication, updateDto))
            .ReturnsAsync(updated);

        // Act
        var result = await _controller.Update(physician, patient, medication, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(updated);
    }

    // =====================================================================
    // 6. UPDATE PRESCRIPTION — NEGATIVE
    // =====================================================================

    [Fact]
    public async Task Update_ShouldThrowNotFoundException_WhenPrescriptionDoesNotExist()
    {
        // Arrange — PrescriptionExistsAsync returns false
        int physician = 1, patient = 999, medication = 201;

        var updateDto = new UpdatePrescriptionDTO
        {
            Physician  = physician,
            Patient    = patient,
            Medication = medication,
            Date       = DateTime.UtcNow,
            Dose       = "500mg"
        };

        _mockService
            .Setup(s => s.PrescriptionExistsAsync(physician, patient, medication))
            .ReturnsAsync(false);

        // Act
        var action = async () => await _controller.Update(physician, patient, medication, updateDto);

        // Assert
        await action.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"*{physician}*{patient}*{medication}*");
    }

    // =====================================================================
    // 7. GET BY DATE RANGE — POSITIVE
    // =====================================================================

    [Fact]
    public async Task GetByDateRange_ShouldReturn200_WhenDateRangeIsValid()
    {
        // Arrange
        var start = DateTime.UtcNow.AddDays(-7);
        var end   = DateTime.UtcNow;

        var list = new List<PrescriptionDTO>
        {
            new() { Physician = 1, Patient = 101, Medication = 201, Date = start.AddDays(1), Dose = "100mg" }
        };

        _mockService
            .Setup(s => s.GetPrescriptionsByDateRangeAsync(start, end))
            .ReturnsAsync(list);

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
        // Arrange — controller validates this before calling service
        var start = DateTime.UtcNow;
        var end   = DateTime.UtcNow.AddDays(-7);   // end is BEFORE start

        // Act
        var action = async () => await _controller.GetByDateRange(start, end);

        // Assert
        await action.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Start date must be earlier than end date");

        _mockService.Verify(s => s.GetPrescriptionsByDateRangeAsync(
            It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
    }
}
