namespace Hms.API.DTOs.Patient;

public class PatientResponseDto
{
    public int Ssn { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public int InsuranceId { get; set; }

    public int Pcp { get; set; }
}