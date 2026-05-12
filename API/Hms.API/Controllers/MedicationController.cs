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
        public async Task<IActionResult> GetAll()
        {
            var medications = await _service.GetAllAsync();

            return Ok(medications);
        }
        [HttpGet("{code}")]
        public async Task<IActionResult> GetById(int code)
        {
            var medication = await _service.GetByIdAsync(code);

            if (medication == null)
                return NotFound(new
                {
                    Message = $"Medication with Code {code} not found"
                });

            return Ok(medication);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicationDto dto)
        {
            var createdMedication = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { code = createdMedication.Code },
                createdMedication);
        }
        [HttpPut("{code}")]
        public async Task<IActionResult> Update(
            int code,
            UpdateMedicationDto dto)
        {
            var updated = await _service.UpdateAsync(code, dto);

            if (!updated)
                return NotFound(new
                {
                    Message = $"Medication with Code {code} not found"
                });

            return Ok(new
            {
                Message = "Medication updated successfully"
            });
        }
                [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(int code)
        {
            var deleted = await _service.DeleteAsync(code);

            if (!deleted)
                return NotFound(new
                {
                    Message = $"Medication with Code {code} not found"
                });

            return Ok(new
            {
                Message = "Medication deleted successfully"
            });
        }
    }
