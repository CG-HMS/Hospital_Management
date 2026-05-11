using Hms.API.DTOs;
using Hms.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StayController : ControllerBase
{
    private readonly IStayService _stayService;

    public StayController(IStayService stayService)
    {
        _stayService = stayService;
    }

    /// <summary>
    /// ENDPOINT 1: Get all stays
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetAll()
    {
        try
        {
            var stays = await _stayService.GetAllStaysAsync();
            return Ok(stays);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 2: Get a specific stay by ID
    /// </summary>
    [HttpGet("{stayId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StayDetailDTO>> GetById(int stayId)
    {
        try
        {
            var stay = await _stayService.GetStayByIdAsync(stayId);
            if (stay == null)
                return NotFound(new { message = "Stay record not found" });

            return Ok(stay);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 3: Get stays by Patient ID
    /// </summary>
    [HttpGet("by-patient/{patientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByPatient(int patientId)
    {
        try
        {
            var stays = await _stayService.GetStaysByPatientAsync(patientId);
            return Ok(stays);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 4: Get stays by Room ID
    /// </summary>
    [HttpGet("by-room/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByRoom(int roomId)
    {
        try
        {
            var stays = await _stayService.GetStaysByRoomAsync(roomId);
            return Ok(stays);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 5: Get all active stays (current date is within stay period)
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetActiveStays()
    {
        try
        {
            var stays = await _stayService.GetActiveStaysAsync();
            return Ok(stays);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 6: Get stays by date range
    /// </summary>
    [HttpGet("by-date-range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
                return BadRequest(new { message = "Start date must be earlier than end date" });

            var stays = await _stayService.GetStaysByDateRangeAsync(startDate, endDate);
            return Ok(stays);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 7: Create a new stay
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StayDTO>> Create([FromBody] CreateStayDTO createStayDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createStayDto.StayStart >= createStayDto.StayEnd)
                return BadRequest(new { message = "Stay start date must be before stay end date" });

            var stay = await _stayService.CreateStayAsync(createStayDto);
            return CreatedAtAction(nameof(GetById), new { stayId = stay.StayId }, stay);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 8: Update an existing stay
    /// </summary>
    [HttpPut("{stayId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StayDTO>> Update(int stayId, [FromBody] UpdateStayDTO updateStayDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (updateStayDto.StayStart >= updateStayDto.StayEnd)
                return BadRequest(new { message = "Stay start date must be before stay end date" });

            var exists = await _stayService.StayExistsAsync(stayId);
            if (!exists)
                return NotFound(new { message = "Stay record not found" });

            var stay = await _stayService.UpdateStayAsync(stayId, updateStayDto);
            return Ok(stay);
        }
        catch (KeyNotFoundException kex)
        {
            return NotFound(new { message = kex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 9: Check if a stay exists
    /// </summary>
    [HttpHead("{stayId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckExists(int stayId)
    {
        try
        {
            var exists = await _stayService.StayExistsAsync(stayId);
            if (!exists)
                return NotFound();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>
    /// ENDPOINT 10: Get stay statistics (count of active stays)
    /// </summary>
    [HttpGet("stats/active-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<object>> GetActiveStaysCount()
    {
        try
        {
            var activeStays = await _stayService.GetActiveStaysAsync();
            var count = activeStays.Count();
            return Ok(new { activeStaysCount = count, timestamp = DateTime.Now });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}
