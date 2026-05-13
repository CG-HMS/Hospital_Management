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
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<IActionResult> GetPatientById(int ssn)
    {
        var patient = await _service.GetPatientByIdAsync(ssn);

        return Ok(patient);
    }

    [HttpPost]
    [Authorize(Roles = "admin,physician")]
    public async Task<IActionResult> CreatePatient(PatientRequestDto dto)
    {
        var patient = await _service.CreatePatientAsync(dto);

        return CreatedAtAction(
            nameof(GetPatientById),
            new { ssn = patient.Ssn },
            patient
        );
    }

    [HttpPut("{ssn}")]
    [Authorize(Roles = "admin,physician")]
    public async Task<IActionResult> UpdatePatient(int ssn, PatientRequestDto dto)
    {
        await _service.UpdatePatientAsync(ssn, dto);

        return Ok(new
        {
            Message = "Patient updated successfully"
        });
    }

    [HttpDelete("{ssn}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeletePatient(int ssn)
    {
        await _service.DeletePatientAsync(ssn);

        return Ok(new
        {
            Message = "Patient deleted successfully"
        });
    }
}