using Hms.API.DTOs.Medication;
using Hms.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MedicationController : ControllerBase
{
    private readonly IMedicationService _service;

    public MedicationController(IMedicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMedications()
    {
        var medications = await _service.GetAllMedicationsAsync();

        return Ok(medications);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetMedicationById(int code)
    {
        try
        {
            var medication = await _service.GetMedicationByIdAsync(code);

            return Ok(medication);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin,physician")]
    public async Task<IActionResult> CreateMedication(MedicationRequestDto dto)
    {
        try
        {
            var medication = await _service.CreateMedicationAsync(dto);

            return CreatedAtAction(
                nameof(GetMedicationById),
                new { code = medication.Code },
                medication
            );
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpPut("{code}")]
    [Authorize(Roles = "admin,physician")]
    public async Task<IActionResult> UpdateMedication(
            int code,
            MedicationRequestDto dto
        )
    {
        try
        {
            await _service.UpdateMedicationAsync(code, dto);

            return Ok(new
            {
                Message = "Medication updated successfully"
            });
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpDelete("{code}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteMedication(int code)
    {
        try
        {
            await _service.DeleteMedicationAsync(code);

            return Ok(new
            {
                Message = "Medication deleted successfully"
            });
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{code}/prescription-count")]
    public async Task<IActionResult> GetPrescriptionCount(int code)
    {
        try
        {
            var count = await _service.GetPrescriptionCountAsync(code);
            return Ok(new { medicationCode = code, prescriptionCount = count });
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("{code}/patients")]
    public async Task<IActionResult> GetMedicationPatients(int code)
    {
        try
        {
            var patients = await _service.GetMedicationPatientsAsync(code);
            return Ok(patients);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTopMedications([FromQuery] int take = 5)
    {
        try
        {
            var medications = await _service.GetTopMedicationsAsync(take);
            return Ok(medications);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }
}