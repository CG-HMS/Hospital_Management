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
    public int Physician { get; set; }

    public int Patient { get; set; }

    public int Medication { get; set; }

    public DateTime Date { get; set; }

    public int? Appointment { get; set; }

    public string Dose { get; set; } = null!;
}

public class UpdatePrescriptionDTO
{
    public int Physician { get; set; }

    public int Patient { get; set; }

    public int Medication { get; set; }

    public DateTime Date { get; set; }

    public int? Appointment { get; set; }
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

