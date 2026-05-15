using Hms.API.DTOs;
using Hms.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "admin,physician,nurse")]
        public async Task<ActionResult<List<AppointmentDto>>> GetAll()
        {
            var appointments = await _service.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "admin,physician,nurse")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            var appointment = await _service.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpGet("filtered")]
        [Authorize(Roles = "admin,physician,nurse")]
        public async Task<ActionResult<List<AppointmentFilterDto>>> GetFiltered([
            FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int? physicianId,
            [FromQuery] int? patientId)
        {
            try
            {
                var appointments = await _service.GetAppointmentsFilteredAsync(fromDate, toDate, physicianId, patientId);
                return Ok(appointments);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("today")]
        [Authorize(Roles = "admin,physician,nurse")]
        public async Task<ActionResult<List<AppointmentFilterDto>>> GetToday([FromQuery] DateTime? date)
        {
            try
            {
                var appointments = await _service.GetTodayAppointmentsAsync(date);
                return Ok(appointments);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("grouped-by-physician")]
        [Authorize(Roles = "admin,physician,nurse")]
        public async Task<ActionResult<List<AppointmentGroupDto>>> GetGroupedByPhysician()
        {
            try
            {
                var grouped = await _service.GetAppointmentsGroupedByPhysicianAsync();
                return Ok(grouped);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin,physician")]
        public async Task<ActionResult<AppointmentDto>> Create([FromBody] AppointmentCreateDto dto)
        {
            try
            {
                var created = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.AppointmentId }, created);
            }
            catch (Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentUpdateDto dto)
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
