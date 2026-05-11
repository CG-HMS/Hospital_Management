using System.ComponentModel.DataAnnotations;

namespace Hms.API.DTOs;

public class PrescriptionDTO
{
    public int Physician { get; set; }
    public int Patient { get; set; }
    public int Medication { get; set; }
    public DateTime Date { get; set; }
    public int? Appointment { get; set; }
    public string Dose { get; set; } = null!;
}

public class CreatePrescriptionDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Physician ID must be greater than 0")]
    public int Physician { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be greater than 0")]
    public int Patient { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Medication ID must be greater than 0")]
    public int Medication { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Appointment ID must be valid")]
    public int? Appointment { get; set; }

    [Required(ErrorMessage = "Dose is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Dose must be between 2 and 100 characters")]
    public string Dose { get; set; } = null!;
}

public class UpdatePrescriptionDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Physician ID must be greater than 0")]
    public int Physician { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be greater than 0")]
    public int Patient { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Medication ID must be greater than 0")]
    public int Medication { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Appointment ID must be valid")]
    public int? Appointment { get; set; }

    [Required(ErrorMessage = "Dose is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Dose must be between 2 and 100 characters")]
    public string Dose { get; set; } = null!;
}

public class PrescriptionDetailDTO
{
    public int Physician { get; set; }
    public int Patient { get; set; }
    public int Medication { get; set; }
    public DateTime Date { get; set; }
    public int? Appointment { get; set; }
    public string Dose { get; set; } = null!;

    public string? PhysicianName { get; set; }
    public string? PatientName { get; set; }
    public string? MedicationName { get; set; }
}

