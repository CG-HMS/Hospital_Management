namespace Hms.API.DTOs.Medication;

 public class MedicationResponseDto
    {
        public int Code { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

public class MedicationUsageDto
{
    public int MedicationCode { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public int PrescriptionCount { get; set; }
}

public class MedicationPatientDto
{
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Dose { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
}
