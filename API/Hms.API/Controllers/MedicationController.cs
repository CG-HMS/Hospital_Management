using Hms.API.DTOs.Medication;
using Hms.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers;

[Route("api/[controller]")]
[ApiController]
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
        var medication = await _service.GetMedicationByIdAsync(code);

        return Ok(medication);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedication(MedicationRequestDto dto)
    {
        var medication = await _service.CreateMedicationAsync(dto);

        return CreatedAtAction(
            nameof(GetMedicationById),
            new { code = medication.Code },
            medication
        );
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> UpdateMedication(
        int code,
        MedicationRequestDto dto
    )
    {
        await _service.UpdateMedicationAsync(code, dto);

        return Ok(new
        {
            Message = "Medication updated successfully"
        });
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteMedication(int code)
    {
        await _service.DeleteMedicationAsync(code);

        return Ok(new
        {
            Message = "Medication deleted successfully"
        });
    }
}