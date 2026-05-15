using Hms.API.DTOs;
using Hms.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _service;

        public NurseController(INurseService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "admin, physician")]
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

        [Authorize(Roles = "nurse")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst("refId");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            //// TEMP DEBUG
            //return Ok(new
            //{
            //    UserId = userId,
            //    Claims = User.Claims.Select(c => new { c.Type, c.Value })
            //});
            var nurse = await _service.GetByIdAsync(userId);

            if (nurse == null)
            {
                return NotFound("Nurse profile not found.");
            }

            return Ok(nurse);
            
        }

        [HttpGet("{id:int}/on-call")]
        [Authorize(Roles = "admin,physician,nurse")]
        public async Task<ActionResult<List<NurseOnCallDto>>> GetOnCallSchedule(int id, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                var schedule = await _service.GetOnCallScheduleAsync(id, fromDate, toDate);
                return Ok(schedule);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}/trained-procedures")]
        [Authorize(Roles = "admin,physician,nurse")]
        public async Task<ActionResult<List<NurseTrainedProcedureDto>>> GetTrainedProcedures(int id)
        {
            try
            {
                var procedures = await _service.GetTrainedProceduresAsync(id);
                return Ok(procedures);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
