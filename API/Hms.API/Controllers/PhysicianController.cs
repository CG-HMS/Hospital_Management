using Hms.API.DTOs;
using Hms.API.Services;
using Hms.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Hms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhysicianController : ControllerBase
    {

        private readonly IPhysicianService _service;
        public PhysicianController(IPhysicianService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhysicians()
        {
            var physicians = await _service.GetAllPhysicians();

            return Ok(physicians);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhysicianById(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var physician = await _service.GetPhysicianById(id);

            return Ok(physician);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddPhysician(PhysicianWriteDto dto)
        {
            var physician = await _service.AddPhysician(dto);

            return Ok(physician);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdatePhysician(int id, PhysicianWriteDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var physician = await _service.UpdatePhysician(id, dto);

            return Ok(physician);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePhysician(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            await _service.DeletePhysician(id);

            return Ok("Physician deleted successfully");
        }

        [HttpGet("{id}/departments")]
        public async Task<IActionResult> GetDepartments(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var departments = await _service.GetDepartmentsByPhysician(id);

            return Ok(departments);
        }

        //[HttpGet("{id}/procedures")]
        //public async Task<IActionResult> GetProcedures(int id)
        //{
        //    if (id <= 0)
        //    {
        //        throw new BadRequestException("Physician ID must be positive.");
        //    }
        //    var procedures = await _service.GetProceduresByPhysician(id);

        //    return Ok(procedures);
        //}

        //[HttpPost("{id}/departments")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> AssignDepartment(int id, AssignDepartmentDto dto)
        //{
        //    if (id <= 0)
        //    {
        //        throw new BadRequestException("Physician ID must be positive.");
        //    }
        //    if (dto.DepartmentId <= 0)
        //    {
        //        throw new BadRequestException("Department ID must be positive.");
        //    }
        //    await _service.AssignDepartment(id, dto);

        //    return Ok("Department assigned successfully");
        //}

        [HttpGet("{id}/appointments")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> GetAppointments(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var appointments = await _service.GetAppointmentsByPhysician(id);

            return Ok(appointments);
        }

        [HttpGet("{id}/appointment-stats")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> GetAppointmentStats(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }

            try
            {
                var stats = await _service.GetAppointmentStatsAsync(id);
                return Ok(stats);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        [HttpGet("{id}/upcoming-appointments")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> GetUpcomingAppointments(int id, [FromQuery] DateTime? fromDate)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }

            try
            {
                var appointments = await _service.GetUpcomingAppointmentsAsync(id, fromDate);
                return Ok(appointments);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        [HttpGet("{id}/patients")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> GetPatients(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var patients = await _service.GetPatientsByPhysician(id);

            return Ok(patients);
        }

        [HttpGet("top")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> GetTopPhysicians([FromQuery] int take = 5)
        {
            try
            {
                var physicians = await _service.GetTopPhysiciansByAppointmentsAsync(take);
                return Ok(physicians);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize(Roles = "physician")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst("refId");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int physicianId = int.Parse(userIdClaim.Value);

            var physician = await _service.GetPhysicianById(physicianId);

            if (physician == null)
            {
                return NotFound("Physician profile not found.");
            }

            return Ok(physician);
        }
    }
}
