using Hms.API.DTOs;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureController : Controller
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
            var procedure = await _service.GetProcedureByCode(code);

            if (procedure == null)
            {
                return NotFound();
            }

            return Ok(procedure);
        }

        [HttpPost]
        public async Task<IActionResult> AddProcedure(CreateProcedureDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var procedure = await _service.AddProcedure(dto);

            return Ok(procedure);
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateProcedure(int code, UpdateProcedureDto dto)
        {
            var procedure = await _service.UpdateProcedure(code, dto);

            if (procedure == null)
            {
                return NotFound();
            }

            return Ok(procedure);
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteProcedure(int code)
        {
            var deleted = await _service.DeleteProcedure(code);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok("Procedure deleted successfully");
        }
        [HttpGet("{code}/physicians")]
        public async Task<IActionResult> GetPhysiciansByProcedure(int code)
        {
            var physicians = await _service.GetPhysiciansByProcedure(code);

            return Ok(physicians);
        }
        [HttpGet("{code}/stays")]
        public async Task<IActionResult> GetStaysByProcedure(int code)
        {
            var stays = await _service.GetStaysByProcedure(code);

            return Ok(stays);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProcedures([FromQuery] string name)
        {
            var procedures = await _service.SearchProcedures(name);

            return Ok(procedures);
        }
        [HttpGet("cost-range")]
        public async Task<IActionResult> GetProceduresByCostRange([FromQuery] float min,[FromQuery] float max)
        {
            if (min > max)
            {
                return BadRequest("Min cost cannot be greater than max cost");
            }

            var procedures = await _service.GetProceduresByCostRange(min,max);

            return Ok(procedures);
        }
    }
}
