using System;
using System.Collections.Generic;

namespace Hms.API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? RefId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
