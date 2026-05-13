

using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAll()
    {
        var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
        return Ok(prescriptions);
    }

    [HttpGet("{physician}/{patient}/{medication}")]
    public async Task<ActionResult<PrescriptionDetailDTO>> GetById(int physician, int patient, int medication)
    {
        var prescription = await _prescriptionService.GetPrescriptionByIdAsync(physician, patient, medication);
        if (prescription == null)
            throw new NotFoundException($"Prescription not found for Physician={physician}, Patient={patient}, Medication={medication}");

        return Ok(prescription);
    }

    [HttpGet("by-physician/{physicianId}")]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByPhysician(int physicianId)
    {
        var prescriptions = await _prescriptionService.GetPrescriptionsByPhysicianAsync(physicianId);
        return Ok(prescriptions);
    }

    [HttpGet("by-patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByPatient(int patientId)
    {
        var prescriptions = await _prescriptionService.GetPrescriptionsByPatientAsync(patientId);
        return Ok(prescriptions);
    }

    [HttpGet("by-medication/{medicationId}")]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByMedication(int medicationId)
    {
        var prescriptions = await _prescriptionService.GetPrescriptionsByMedicationAsync(medicationId);
        return Ok(prescriptions);
    }

    [HttpGet("by-appointment/{appointmentId}")]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByAppointment(int appointmentId)
    {
        var prescriptions = await _prescriptionService.GetPrescriptionsByAppointmentAsync(appointmentId);
        return Ok(prescriptions);
    }

    [HttpGet("by-date-range")]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            throw new BadRequestException("Start date must be earlier than end date");

        var prescriptions = await _prescriptionService.GetPrescriptionsByDateRangeAsync(startDate, endDate);
        return Ok(prescriptions);
    }

    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetRecent([FromQuery] int days = 7)
    {
        if (days <= 0)
            throw new BadRequestException("Days must be greater than 0");

        var prescriptions = await _prescriptionService.GetRecentPrescriptionsAsync(days);
        return Ok(prescriptions);
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionDTO>> Create([FromBody] CreatePrescriptionDTO createPrescriptionDto)
    {
        var prescription = await _prescriptionService.CreatePrescriptionAsync(createPrescriptionDto);
        return CreatedAtAction(nameof(GetById), 
            new { physician = prescription.Physician, patient = prescription.Patient, medication = prescription.Medication }, 
            prescription);
    }

    [HttpPut("{physician}/{patient}/{medication}")]
    public async Task<ActionResult<PrescriptionDTO>> Update(int physician, int patient, int medication, [FromBody] UpdatePrescriptionDTO updatePrescriptionDto)
    {
        var exists = await _prescriptionService.PrescriptionExistsAsync(physician, patient, medication);
        if (!exists)
            throw new NotFoundException($"Prescription not found for Physician={physician}, Patient={patient}, Medication={medication}");

        var prescription = await _prescriptionService.UpdatePrescriptionAsync(physician, patient, medication, updatePrescriptionDto);
        return Ok(prescription);
    }
}