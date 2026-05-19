using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Nurse;

public class NurseViewModel
{
    [Required(ErrorMessage = "Employee ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [RegularExpression("^[A-Za-z].*$", ErrorMessage = "Name must start with a letter")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position is required")]
    [StringLength(100, ErrorMessage = "Position cannot exceed 100 characters")]
    public string Position { get; set; } = string.Empty;

    public bool Registered { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int? Ssn { get; set; }
}
