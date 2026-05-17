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

public class AppointmentFilterDto
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

public class AppointmentGroupDto
{
    public int PhysicianId { get; set; }
    public string PhysicianName { get; set; } = string.Empty;
    public int AppointmentCount { get; set; }
}
