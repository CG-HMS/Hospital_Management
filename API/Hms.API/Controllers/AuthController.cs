using Hms.API.DTOs;
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

        // POST api/auth/login  — PUBLIC
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }

        // GET api/auth/users  — ADMIN
        [HttpGet("users")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _authService.GetAllUsersAsync();
            return Ok(response);
        }

        // GET api/auth/users/{id}  — ADMIN
        [HttpGet("users/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _authService.GetUserByIdAsync(id);
            return Ok(response);
        }

        // POST api/auth/users  — ADMIN
        [HttpPost("users")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var response = await _authService.CreateUserAsync(dto);
            return StatusCode(201, response);
        }

        // PATCH api/auth/users/{id}/change-password  — ADMIN
        [HttpPatch("users/{id:int}/change-password")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordDto dto)
        {
            await _authService.ChangePasswordAsync(id, dto);
            return Ok(new { message = "Password changed successfully." });
        }

        // PATCH api/auth/users/{id}/role  — ADMIN
        [HttpPatch("users/{id:int}/role")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto dto)
        {
            await _authService.UpdateRoleAsync(id, dto.Role);
            return Ok(new { message = "Role updated successfully." });
        }

        // PATCH api/auth/users/{id}/status  — ADMIN
        [HttpPatch("users/{id:int}/status")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto dto)
        {
            await _authService.UpdateStatusAsync(id, dto.IsActive);
            return Ok(new { message = $"Account {(dto.IsActive ? "activated" : "deactivated")} successfully." });
        }

        //// DELETE api/auth/users/{id}  — ADMIN
        //[HttpDelete("users/{id:int}")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    await _authService.DeleteUserAsync(id);
        //    return NoContent();
        //}
    }
}
