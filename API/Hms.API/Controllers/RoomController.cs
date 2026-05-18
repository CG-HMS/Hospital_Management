using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService) => _roomService = roomService;

        // ── GET /api/rooms  — ANY ROLE ────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomService.GetAllAsync();
            return Ok(result);
        }

        // ── GET /api/rooms/available  — ANY ROLE ──────────────────────────────
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _roomService.GetAvailableAsync();
            return Ok(result);
        }

        [HttpGet("occupied")]
        public async Task<IActionResult> GetOccupied()
        {
            try
            {
                var rooms = await _roomService.GetOccupiedRoomsAsync();
                return Ok(rooms);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        // ── GET /api/rooms/{id}  — ANY ROLE ───────────────────────────────────
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) throw new BadRequestException("Room number must be a positive integer.");

            var result = await _roomService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetRoomHistory(int id)
        {
            if (id <= 0) throw new BadRequestException("Room number must be a positive integer.");

            try
            {
                var history = await _roomService.GetRoomPatientHistoryAsync(id);
                return Ok(history);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        // ── GET /api/rooms/block/{floor}/{code}  — ANY ROLE ───────────────────
        [HttpGet("block/{floor:int}/{code:int}")]
        public async Task<IActionResult> GetByBlock(int floor, int code)
        {
            if (floor <= 0) throw new BadRequestException("Block floor must be a positive integer.");
            if (code < 0) throw new BadRequestException("Block code must be zero or positive.");

            var result = await _roomService.GetByBlockAsync(floor, code);
            return Ok(result);
        }

        // ── GET /api/rooms/type/{type}  — ANY ROLE ────────────────────────────
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new BadRequestException("Room type cannot be empty.");

            var result = await _roomService.GetByTypeAsync(type);
            return Ok(result);
        }

        [HttpGet("utilization")]
        public async Task<IActionResult> GetUtilization()
        {
            try
            {
                var utilization = await _roomService.GetRoomUtilizationAsync();
                return Ok(utilization);
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        // ── POST /api/rooms/{roomNumber}  — ADMIN ─────────────────────────────
        [HttpPost("{roomNumber:int}")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> Create(int roomNumber, [FromBody] RoomWriteDto dto)
        {
            if (roomNumber <= 0) throw new BadRequestException("Room number must be a positive integer.");

            var result = await _roomService.CreateAsync(roomNumber, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.RoomNumber }, result);
        }

        // ── PUT /api/rooms/{id}  — ADMIN ──────────────────────────────────────
        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomWriteDto dto)
        {
            if (id <= 0) throw new BadRequestException("Room number must be a positive integer.");

            var result = await _roomService.UpdateAsync(id, dto);
            return Ok(result);
        }

        // ── PATCH /api/rooms/{id}/availability  — ADMIN ───────────────────────
        [HttpPatch("{id:int}/availability")]
        [Authorize(Roles = "admin,physician")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] bool unavailable)
        {
            if (id <= 0) throw new BadRequestException("Room number must be a positive integer.");

            await _roomService.UpdateAvailabilityAsync(id, unavailable);
            return Ok(new { message = $"Room {id} marked as {(unavailable ? "unavailable" : "available")}." });
        }
    }
}