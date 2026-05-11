using Hms.API.DTOs;
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

    /// <summary>
    /// ENDPOINT 1: Get all prescriptions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAll()
    {
        try
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 2: Get a specific prescription by composite key (Physician, Patient, Medication)
    /// </summary>
    [HttpGet("{physician}/{patient}/{medication}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PrescriptionDetailDTO>> GetById(int physician, int patient, int medication)
    {
        try
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(physician, patient, medication);
            if (prescription == null)
                return NotFound(new { message = "Prescription record not found" });

            return Ok(prescription);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 3: Get prescriptions by Physician ID
    /// </summary>
    [HttpGet("by-physician/{physicianId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByPhysician(int physicianId)
    {
        try
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByPhysicianAsync(physicianId);
            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 4: Get prescriptions by Patient ID
    /// </summary>
    [HttpGet("by-patient/{patientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByPatient(int patientId)
    {
        try
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByPatientAsync(patientId);
            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 5: Get prescriptions by Medication ID
    /// </summary>
    [HttpGet("by-medication/{medicationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByMedication(int medicationId)
    {
        try
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByMedicationAsync(medicationId);
            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 6: Get prescriptions by Appointment ID
    /// </summary>
    [HttpGet("by-appointment/{appointmentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByAppointment(int appointmentId)
    {
        try
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByAppointmentAsync(appointmentId);
            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 7: Get prescriptions by date range
    /// </summary>
    [HttpGet("by-date-range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
                return BadRequest(new { message = "Start date must be earlier than end date" });

            var prescriptions = await _prescriptionService.GetPrescriptionsByDateRangeAsync(startDate, endDate);
            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 8: Get recent prescriptions (last N days)
    /// </summary>
    [HttpGet("recent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetRecent([FromQuery] int days = 7)
    {
        try
        {
            if (days <= 0)
                return BadRequest(new { message = "Days must be greater than 0" });

            var prescriptions = await _prescriptionService.GetRecentPrescriptionsAsync(days);
            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 9: Create a new prescription
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PrescriptionDTO>> Create([FromBody] CreatePrescriptionDTO createPrescriptionDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var prescription = await _prescriptionService.CreatePrescriptionAsync(createPrescriptionDto);
            return CreatedAtAction(nameof(GetById), 
                new { physician = prescription.Physician, patient = prescription.Patient, medication = prescription.Medication }, 
                prescription);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 10: Update an existing prescription
    /// </summary>
    [HttpPut("{physician}/{patient}/{medication}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PrescriptionDTO>> Update(int physician, int patient, int medication, [FromBody] UpdatePrescriptionDTO updatePrescriptionDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _prescriptionService.PrescriptionExistsAsync(physician, patient, medication);
            if (!exists)
                return NotFound(new { message = "Prescription record not found" });

            var prescription = await _prescriptionService.UpdatePrescriptionAsync(physician, patient, medication, updatePrescriptionDto);
            return Ok(prescription);
        }
        catch (KeyNotFoundException kex)
        {
            return NotFound(new { message = kex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}
