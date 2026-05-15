namespace Hms.API.DTOs.Physician;

public class PhysicianAppointmentStatsDto
{
    public int PhysicianId { get; set; }
    public int TotalAppointments { get; set; }
    public DateTime? LastAppointmentDate { get; set; }
}
