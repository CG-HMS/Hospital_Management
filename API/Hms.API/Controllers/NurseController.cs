using Hms.API.DTOs;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _service;

        public NurseController(INurseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<NurseDto>>> GetAll()
        {
            var nurses = await _service.GetAllAsync();
            return Ok(nurses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<NurseDto>> GetById(int id)
        {
            var nurse = await _service.GetByIdAsync(id);
            if (nurse == null)
            {
                return NotFound();
            }

            return Ok(nurse);
        }

        [HttpPost]
        public async Task<ActionResult<NurseDto>> Create(NurseDto dto)
        {
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, NurseDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
