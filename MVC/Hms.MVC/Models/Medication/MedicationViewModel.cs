using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Medication;

public class MedicationViewModel
{
    [Required(ErrorMessage = "Medication code is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Code { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
    public string Brand { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;
}
