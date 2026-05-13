using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProcedureController : ControllerBase
    {
        private readonly IProcedureService _service;

        public ProcedureController(IProcedureService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProcedures()
        {
            var procedures = await _service.GetAllProcedures();

            return Ok(procedures);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetProcedure(int code)
        {
            if (code <= 0)
            {
                throw new BadRequestException("Procedure code must be positive.");
            }
            var procedure = await _service.GetProcedureByCode(code);

            return Ok(procedure);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProcedure(ProcedureWriteDto dto)
        {
            var procedure = await _service.AddProcedure(dto);

            return Ok(procedure);
        }

        [HttpPut("{code}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProcedure(int code, ProcedureWriteDto dto)
        {
            if (code <= 0)
            {
                throw new BadRequestException("Procedure code must be positive.");
            }
            var procedure = await _service.UpdateProcedure(code, dto);

            return Ok(procedure);
        }

        [HttpDelete("{code}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProcedure(int code)
        {
            if (code <= 0)
            {
                throw new BadRequestException("Procedure code must be positive.");
            }
            await _service.DeleteProcedure(code);

            return Ok("Procedure deleted successfully");
        }
        [HttpGet("{code}/physicians")]
        public async Task<IActionResult> GetPhysiciansByProcedure(int code)
        {
            if (code <= 0)
            {
                throw new BadRequestException("Procedure code must be positive.");
            }
            var physicians = await _service.GetPhysiciansByProcedure(code);

            return Ok(physicians);
        }

        [HttpGet("{code}/stays")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> GetStaysByProcedure(int code)
        {
            if (code <= 0)
            {
                throw new BadRequestException("Procedure code must be positive.");
            }
            var stays = await _service.GetStaysByProcedure(code);

            return Ok(stays);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProcedures([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BadRequestException("Procedure name cannot be empty.");
            }
            var procedures = await _service.SearchProcedures(name);

            return Ok(procedures);
        }

        [HttpGet("cost-range")]
        public async Task<IActionResult> GetProceduresByCostRange([FromQuery] float min, [FromQuery] float max)
        {
            if (min < 0 || max < 0)
            {
                throw new BadRequestException("Cost cannot be negative.");
            }
            if (min > max)
            {
                throw new BadRequestException("Minimum cost cannot be greater than maximum cost.");

            }
            var procedures = await _service.GetProceduresByCostRange(min, max);

            return Ok(procedures);
        }
    }
}
