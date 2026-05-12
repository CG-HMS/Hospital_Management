namespace Hms.API.DTOs;

public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public int Patient { get; set; }
    public int? PrepNurse { get; set; }
    public int Physician { get; set; }
    public DateTime Starto { get; set; }
    public DateTime Endo { get; set; }
    public string ExaminationRoom { get; set; } = null!;
}

public class AppointmentEditDto
{
    public int Patient { get; set; }
    public int? PrepNurse { get; set; }
    public int Physician { get; set; }
    public DateTime Starto { get; set; }
    public DateTime Endo { get; set; }
    public string ExaminationRoom { get; set; } = null!;
}

public class AppointmentCreateDto : AppointmentEditDto
{
}

public class AppointmentUpdateDto : AppointmentEditDto
{
}
