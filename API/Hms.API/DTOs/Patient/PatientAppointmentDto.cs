namespace Hms.API.DTOs.Patient;

public class PatientAppointmentDto
{
    public int AppointmentId { get; set; }
    public DateTime Starto { get; set; }
    public DateTime Endo { get; set; }
    public string ExaminationRoom { get; set; } = string.Empty;
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
}
