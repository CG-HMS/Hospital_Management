namespace Hms.API.DTOs.Patient;

public class PatientDashboardDto
{
    public int AppointmentCount { get; set; }
    public int MedicationCount { get; set; }
    public int StayCount { get; set; }
    public int ProcedureCount { get; set; }
    public DateTime? LastAppointmentDate { get; set; }
}
