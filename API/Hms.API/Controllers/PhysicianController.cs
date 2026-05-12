using Hms.API.DTOs;
using Hms.API.Services;
using Hms.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async 
Task<IActionResult> AddPhysician(PhysicianWriteDto dto)
        {
            var physician = await _service.AddPhysician(dto);

            return Ok(physician);
        }

        [HttpPut("{id}")]
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

        [HttpGet("{id}/procedures")]
        public async Task<IActionResult> GetProcedures(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var procedures = await _service.GetProceduresByPhysician(id);

            return Ok(procedures);
        }

        [HttpPost("{id}/departments")]
        public async Task<IActionResult> AssignDepartment(int id, AssignDepartmentDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            if (dto.DepartmentId <= 0)
            {
                throw new BadRequestException("Department ID must be positive.");
            }
            await _service.AssignDepartment(id, dto);

            return Ok("Department assigned successfully");
        }
        
        [HttpGet("{id}/appointments")]
        public async Task<IActionResult> GetAppointments(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var appointments = await _service.GetAppointmentsByPhysician(id);

            return Ok(appointments);
        }

        [HttpGet("{id}/patients")]
        public async Task<IActionResult> GetPatients(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Physician ID must be positive.");
            }
            var patients = await _service.GetPatientsByPhysician(id);

            return Ok(patients);
        }
    }
}
