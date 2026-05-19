using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Appointment;

public class AppointmentViewModel
{
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = "Patient ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Patient { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int? PrepNurse { get; set; }

    [Required(ErrorMessage = "Physician ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Physician { get; set; }

    [Required(ErrorMessage = "Start time is required")]
    public DateTime Starto { get; set; }

    [Required(ErrorMessage = "End time is required")]
    public DateTime Endo { get; set; }

    [Required(ErrorMessage = "Examination room is required")]
    [StringLength(50, ErrorMessage = "Room cannot exceed 50 characters")]
    public string ExaminationRoom { get; set; } = string.Empty;
}

public class AppointmentCreateViewModel
{
    [Required(ErrorMessage = "Patient ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Patient { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int? PrepNurse { get; set; }

    [Required(ErrorMessage = "Physician ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Physician { get; set; }

    [Required(ErrorMessage = "Start time is required")]
    public DateTime Starto { get; set; }

    [Required(ErrorMessage = "End time is required")]
    public DateTime Endo { get; set; }

    [Required(ErrorMessage = "Examination room is required")]
    [StringLength(50, ErrorMessage = "Room cannot exceed 50 characters")]
    public string ExaminationRoom { get; set; } = string.Empty;
}

public class AppointmentFilterViewModel
{
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int AppointmentId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int PatientId { get; set; }

    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string PatientName { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int PhysicianId { get; set; }

    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string PhysicianName { get; set; } = string.Empty;

    public DateTime Starto { get; set; }

    public DateTime Endo { get; set; }

    [StringLength(50, ErrorMessage = "Room cannot exceed 50 characters")]
    public string ExaminationRoom { get; set; } = string.Empty;
}
