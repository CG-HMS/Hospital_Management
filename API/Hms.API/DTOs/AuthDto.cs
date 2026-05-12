namespace Hms.API.DTOs
{
    // ── Login ──────────────────────────────────────────────────────────────────
    public class LoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }

    // ── User response ──────────────────────────────────────────────────────────
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? RefId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ── Create user (POST only — UserId is never accepted from client) ─────────
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? RefId { get; set; }
    }

    // ── Change password (PATCH /change-password only) ─────────────────────────
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    // UpdateRoleDto   → REMOVED  — controller uses [FromBody] string role
    // UpdateStatusDto → REMOVED  — controller uses [FromBody] bool isActive
}