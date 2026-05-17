namespace Hms.MVC.Models.Appointment;

public class AppointmentViewModel
{
    public int AppointmentId { get; set; }
    public int Patient { get; set; }
    public int? PrepNurse { get; set; }
    public int Physician { get; set; }
    public DateTime Starto { get; set; }
    public DateTime Endo { get; set; }
    public string ExaminationRoom { get; set; } = string.Empty;
}

public class AppointmentFilterViewModel
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
    public DateTime Starto { get; set; }
    public DateTime Endo { get; set; }
    public string ExaminationRoom { get; set; } = string.Empty;
}
