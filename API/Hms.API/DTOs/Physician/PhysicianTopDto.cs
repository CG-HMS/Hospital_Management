namespace Hms.API.DTOs.Physician;

public class PhysicianTopDto
{
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
    public int AppointmentCount { get; set; }
}
