using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ── POST /api/auth/login  — PUBLIC ────────────────────────────────────
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }

        // ── GET /api/auth/users  — ADMIN ──────────────────────────────────────
        [HttpGet("users")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _authService.GetAllUsersAsync();
            return Ok(response);
        }

        // ── GET /api/auth/users/{id}  — ADMIN ────────────────────────────────
        [HttpGet("users/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0) throw new BadRequestException("User ID must be a positive number.");

            var response = await _authService.GetUserByIdAsync(id);
            return Ok(response);
        }

        // ── POST /api/auth/users  — ADMIN ────────────────────────────────────
        [HttpPost("users")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var response = await _authService.CreateUserAsync(dto);
            return StatusCode(201, response);
        }

        // ── PATCH /api/auth/users/{id}/change-password  — ADMIN ───────────────
        [HttpPatch("users/{id:int}/change-password")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto dto)
        {
            if (id <= 0) throw new BadRequestException("User ID must be a positive number.");

            await _authService.ChangePasswordAsync(id, dto);
            return Ok(new { message = "Password changed successfully." });
        }

        // ── PATCH /api/auth/users/{id}/role  — ADMIN ──────────────────────────
        [HttpPatch("users/{id:int}/role")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string role)
        {
            if (id <= 0) throw new BadRequestException("User ID must be a positive number.");
            if (string.IsNullOrWhiteSpace(role)) throw new BadRequestException("Role cannot be empty.");

            await _authService.UpdateRoleAsync(id, role);
            return Ok(new { message = "Role updated successfully." });
        }

        // ── PATCH /api/auth/users/{id}/status  — ADMIN ────────────────────────
        [HttpPatch("users/{id:int}/status")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isActive)
        {
            if (id <= 0) throw new BadRequestException("User ID must be a positive number.");

            await _authService.UpdateStatusAsync(id, isActive);
            return Ok(new { message = $"Account {(isActive ? "activated" : "deactivated")} successfully." });
        }
    }
}