namespace Hms.API.DTOs.Medication;

public class MedicationRequestDto
{
    public int Code { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}