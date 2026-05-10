using Hms.API.DTOs;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhysicianController : Controller
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
            var physician = await _service.GetPhysicianById(id);
            if (physician == null)
            {
                return NotFound("Physician not found");
            }
            return Ok(physician);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhysician(CreatePhysicianDto dto)
        {
            var physician = await _service.AddPhysician(dto);

            return Ok(physician);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhysician(int id, UpdatePhysicianDto dto)
        {
            var physician = await _service.UpdatePhysician(id, dto);
            if (physician == null)
            {
                return NotFound("Physician not found");
            }
            return Ok(physician);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhysician(int id)
        {
            var deleted = await _service.DeletePhysician(id);
            if (!deleted)
            {
                return NotFound("Physician not found");
            }
            return Ok("Physician deleted successfully");
        }

        [HttpGet("{id}/departments")]
        public async Task<IActionResult> GetDepartments(int id)
        {
            var departments = await _service.GetDepartmentsByPhysician(id);

            return Ok(departments);
        }

        [HttpGet("{id}/procedures")]
        public async Task<IActionResult> GetProcedures(int id)
        {
            var procedures = await _service.GetProceduresByPhysician(id);

            return Ok(procedures);
        }
        [HttpPost("{id}/departments")]
        public async Task<IActionResult> AssignDepartment(
    int id,
    AssignDepartmentDto dto)
        {
            var assigned = await _service.AssignDepartment(id, dto);

            if (!assigned)
            {
                return BadRequest("Invalid physician or department");
            }

            return Ok("Department assigned successfully");
        }
        [HttpGet("{id}/appointments")]
        public async Task<IActionResult> GetAppointments(int id)
        {
            var appointments = await _service.GetAppointmentsByPhysician(id);

            return Ok(appointments);
        }
        [HttpGet("{id}/patients")]
        public async Task<IActionResult> GetPatients(int id)
        {
            var patients = await _service.GetPatientsByPhysician(id);

            return Ok(patients);
        }
    }
}
