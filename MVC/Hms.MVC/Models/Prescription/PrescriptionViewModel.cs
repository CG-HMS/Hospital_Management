using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Prescription;

public class PrescriptionViewModel
{
    [Required(ErrorMessage = "Physician ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Physician { get; set; }

    [Required(ErrorMessage = "Patient ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Patient { get; set; }

    [Required(ErrorMessage = "Medication ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Medication { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int? Appointment { get; set; }

    [Required(ErrorMessage = "Dose is required")]
    [StringLength(100, ErrorMessage = "Dose cannot exceed 100 characters")]
    public string Dose { get; set; } = string.Empty;
}
