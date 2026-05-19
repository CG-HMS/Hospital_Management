using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Patient;

public class PatientViewModel
{
    [Required(ErrorMessage = "SSN is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Ssn { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [RegularExpression("^[A-Za-z].*$", ErrorMessage = "Name must start with a letter")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insurance ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int InsuranceId { get; set; }

    [Required(ErrorMessage = "Primary care physician ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Pcp { get; set; }
}

public class PatientDetailsViewModel : PatientViewModel
{
    // Additional properties like appointments, history, etc. could go here
    // based on what the API returns or what is needed in the view.
}