namespace Hms.API.DTOs
{
    public class PhysicianDto
    {
        public int EmployeeId { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public string Position { get; set; }
            = string.Empty;
    }

    public class PhysicianWriteDto
    {
        public string? Name { get; set; }

        public string? Position { get; set; }

        public int? Ssn { get; set; }
    }

    public class AssignDepartmentDto
    {
        public int DepartmentId { get; set; }
    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }
            = string.Empty;
    }

    public class PatientDto
    {
        public int Ssn { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public string Address { get; set; }
            = string.Empty;

        public string Phone { get; set; }
            = string.Empty;
    }

    public class AppointDto
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }
            = string.Empty;

        public DateTime StartDateTime { get; set; }

        public string ExaminationRoom { get; set; }
            = string.Empty;
    }
}