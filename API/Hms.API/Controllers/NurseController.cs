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
        public async Task<ActionResult<NurseDto>> Create(NurseCreateDto dto)
        {
            try
            {
                var created = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, NurseUpdateDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (Exceptions.NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exceptions.NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
