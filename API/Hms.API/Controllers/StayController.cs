
using Hms.API.DTOs;
using Hms.API.Exceptions;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetAll()
    {
        var stays = await _stayService.GetAllStaysAsync();
        return Ok(stays);
    }

    [HttpGet("{stayId}")]
    public async Task<ActionResult<StayDetailDTO>> GetById(int stayId)
    {
        var stay = await _stayService.GetStayByIdAsync(stayId);
        if (stay == null)
            throw new NotFoundException("Stay", stayId);

        return Ok(stay);
    }

    [HttpGet("by-patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByPatient(int patientId)
    {
        var stays = await _stayService.GetStaysByPatientAsync(patientId);
        return Ok(stays);
    }

    [HttpGet("by-room/{roomId}")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByRoom(int roomId)
    {
        var stays = await _stayService.GetStaysByRoomAsync(roomId);
        return Ok(stays);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetActiveStays()
    {
        var stays = await _stayService.GetActiveStaysAsync();
        return Ok(stays);
    }

    [HttpGet("by-date-range")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            throw new BadRequestException("Start date must be earlier than end date");

        var stays = await _stayService.GetStaysByDateRangeAsync(startDate, endDate);
        return Ok(stays);
    }

    [HttpPost]
    public async Task<ActionResult<StayDTO>> Create([FromBody] CreateStayDTO createStayDto)
    {
        var stay = await _stayService.CreateStayAsync(createStayDto);
        return CreatedAtAction(nameof(GetById), new { stayId = stay.StayId }, stay);
    }

    [HttpPut("{stayId}")]
    public async Task<ActionResult<StayDTO>> Update(int stayId, [FromBody] UpdateStayDTO updateStayDto)
    {
        var exists = await _stayService.StayExistsAsync(stayId);
        if (!exists)
            throw new NotFoundException("Stay", stayId);

        var stay = await _stayService.UpdateStayAsync(stayId, updateStayDto);
        return Ok(stay);
    }

    [HttpHead("{stayId}")]
    public async Task<IActionResult> CheckExists(int stayId)
    {
        var exists = await _stayService.StayExistsAsync(stayId);
        if (!exists)
            return NotFound();

        return Ok();
    }

    [HttpGet("stats/active-count")]
    public async Task<ActionResult<object>> GetActiveStaysCount()
    {
        var activeStays = await _stayService.GetActiveStaysAsync();
        var count = activeStays.Count();
        return Ok(new { activeStaysCount = count, timestamp = DateTime.UtcNow });
    }
}