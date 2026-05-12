using Hms.API.DTOs.Patient;
using Hms.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPatients()
    {
        var patients = await _service.GetAllPatientsAsync();

        return Ok(patients);
    }
    [HttpGet("{ssn}")]
    public async Task<IActionResult> GetPatientById(int ssn)
    {
        var patient = await _service.GetPatientByIdAsync(ssn);

        if (patient == null)
            return NotFound(new
            {
                Message = "Patient not found"
            });

        return Ok(patient);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient(CreatePatientDto dto)
    {
        var patient = await _service.CreatePatientAsync(dto);

        return CreatedAtAction(
            nameof(GetPatientById),
            new { ssn = patient.Ssn },
            patient
        );
    }
    [HttpPut("{ssn}")]
    public async Task<IActionResult> UpdatePatient(int ssn, UpdatePatientDto dto)
    {
        var updated = await _service.UpdatePatientAsync(ssn, dto);

        if (!updated)
            return NotFound(new
            {
                Message = "Patient not found"
            });

        return Ok(new
        {
            Message = "Patient updated successfully"
        });
    }

    [HttpDelete("{ssn}")]
    public async Task<IActionResult> DeletePatient(int ssn)
    {
        var deleted = await _service.DeletePatientAsync(ssn);

        if (!deleted)
            return NotFound(new
            {
                Message = "Patient not found"
            });
                    return Ok(new
        {
            Message = "Patient deleted successfully"
        });
    }
}