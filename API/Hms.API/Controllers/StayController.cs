
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StayController : ControllerBase
{
    private readonly IStayService _stayService;

    public StayController(IStayService stayService)
    {
        _stayService = stayService;
    }

    [HttpGet]
    [Authorize(Roles = "admin,physician,nurse")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetAll()
    {
        var stays = await _stayService.GetAllStaysAsync();
        return Ok(stays);
    }

    [HttpGet("{stayId}")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<ActionResult<StayDetailDTO>> GetById(int stayId)
    {
        var stay = await _stayService.GetStayByIdAsync(stayId);
        if (stay == null)
            throw new NotFoundException("Stay", stayId);

        return Ok(stay);
    }

    [HttpGet("by-patient/{patientId}")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByPatient(int patientId)
    {
        var stays = await _stayService.GetStaysByPatientAsync(patientId);
        return Ok(stays);
    }

    [HttpGet("by-patient/{patientId}/current-room")]
    [Authorize(Roles = "admin,physician,nurse,patient")]
    public async Task<ActionResult<StayCurrentRoomDto>> GetCurrentRoom(int patientId)
    {
        try
        {
            var room = await _stayService.GetCurrentRoomAsync(patientId);
            return Ok(room);
        }
        catch (Exceptions.AppException ex)
        {
            return StatusCode(ex.StatusCode, new { message = ex.Message });
        }
    }

    [HttpGet("by-room/{roomId}")]
    [Authorize(Roles = "admin,nurse")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByRoom(int roomId)
    {
        var stays = await _stayService.GetStaysByRoomAsync(roomId);
        return Ok(stays);
    }

    [HttpGet("active")]
    [Authorize(Roles = "admin,physician,nurse")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetActiveStays()
    {
        var stays = await _stayService.GetActiveStaysAsync();
        return Ok(stays);
    }

    [HttpGet("by-date-range")]
    [Authorize(Roles = "admin,physician")]
    public async Task<ActionResult<IEnumerable<StayDTO>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            throw new BadRequestException("Start date must be earlier than end date");

        var stays = await _stayService.GetStaysByDateRangeAsync(startDate, endDate);
        return Ok(stays);
    }

    [HttpPost]
    [Authorize(Roles = "admin,physician")]
    public async Task<ActionResult<StayDTO>> Create([FromBody] CreateStayDTO createStayDto)
    {
        var stay = await _stayService.CreateStayAsync(createStayDto);
        return CreatedAtAction(nameof(GetById), new { stayId = stay.StayId }, stay);
    }

    [HttpPut("{stayId}")]
    [Authorize(Roles = "admin,physician")]
    public async Task<ActionResult<StayDTO>> Update(int stayId, [FromBody] UpdateStayDTO updateStayDto)
    {
        var exists = await _stayService.StayExistsAsync(stayId);
        if (!exists)
            throw new NotFoundException("Stay", stayId);

        var stay = await _stayService.UpdateStayAsync(stayId, updateStayDto);
        return Ok(stay);
    }

    [HttpGet("stats/active-count")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> GetActiveStaysCount()
    {
        var activeStays = await _stayService.GetActiveStaysAsync();
        var count = activeStays.Count();
        return Ok(new { activeStaysCount = count, timestamp = DateTime.UtcNow });
    }
}