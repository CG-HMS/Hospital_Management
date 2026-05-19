using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Auth;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be 3-50 characters.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Links to PhysicianId / NurseId / Patient SSN depending on role.
    /// Leave null for admin accounts.
    /// </summary>
    public int? RefId { get; set; }

    public string? ErrorMessage { get; set; }
}
