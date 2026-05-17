namespace Hms.API.DTOs.Patient;

public class PatientMedicationDto
{
    public int MedicationCode { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string Dose { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
}
