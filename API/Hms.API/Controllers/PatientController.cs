using Hms.API.DTOs.Patient;
using Hms.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "admin,physician,nurse")]
    public async Task<IActionResult> GetAllPatients()
    {
        var patients = await _service.GetAllPatientsAsync();

        return Ok(patients);
    }

    [HttpGet("{ssn}")]
    [Authorize(Roles = "admin,physician,nurse")]
    public async Task<IActionResult> GetPatientById(int ssn)
    {
        try
        {
            var patient = await _service.GetPatientByIdAsync(ssn);

            return Ok(patient);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin,physician")]
    public async Task<IActionResult> CreatePatient(PatientRequestDto dto)
    {
        try
        {
            var patient = await _service.CreatePatientAsync(dto);

            return CreatedAtAction(
                nameof(GetPatientById),
                new { ssn = patient.Ssn },
                patient
            );
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPut("{ssn}")]
    [Authorize(Roles = "admin,physician")]
    public async Task<IActionResult> UpdatePatient(int ssn, PatientRequestDto dto)
    {
        try
        {
            await _service.UpdatePatientAsync(ssn, dto);

            return Ok(new
            {
                Message = "Patient updated successfully"
            });
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpDelete("{ssn}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeletePatient(int ssn)
    {
        try
        {
            await _service.DeletePatientAsync(ssn);

            return Ok(new
            {
                Message = "Patient deleted successfully"
            });
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [Authorize(Roles = "patient")]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst("refId");

        if (userIdClaim == null)
        {
            return Unauthorized("User ID not found in token.");
        }

        int ssn = int.Parse(userIdClaim.Value);

        try
        {
            var patient = await _service.GetPatientByIdAsync(ssn);

            return Ok(patient);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{ssn}/appointments")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<IActionResult> GetPatientAppointments(int ssn)
    {
        try
        {
            var appointments = await _service.GetAppointmentsByPatientAsync(ssn);
            return Ok(appointments);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{ssn}/medications")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<IActionResult> GetPatientMedications(int ssn)
    {
        try
        {
            var medications = await _service.GetMedicationsByPatientAsync(ssn);
            return Ok(medications);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{ssn}/stays")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<IActionResult> GetPatientStays(int ssn)
    {
        try
        {
            var stays = await _service.GetStayHistoryByPatientAsync(ssn);
            return Ok(stays);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{ssn}/procedures")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<IActionResult> GetPatientProcedures(int ssn)
    {
        try
        {
            var procedures = await _service.GetProceduresByPatientAsync(ssn);
            return Ok(procedures);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{ssn}/dashboard")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<IActionResult> GetPatientDashboard(int ssn)
    {
        try
        {
            var dashboard = await _service.GetPatientDashboardAsync(ssn);
            return Ok(dashboard);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }
}