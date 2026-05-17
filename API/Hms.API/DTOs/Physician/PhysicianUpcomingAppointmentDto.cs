namespace Hms.API.DTOs.Physician;

public class PhysicianUpcomingAppointmentDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime Starto { get; set; }
    public string ExaminationRoom { get; set; } = string.Empty;
}
