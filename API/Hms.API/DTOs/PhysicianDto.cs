namespace Hms.API.DTOs
{
    public class PhysicianDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    }
    public class CreatePhysicianDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int Ssn { get; set; }
    }

    public class UpdatePhysicianDto
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
    }
    public class PatientDto
    {
        public int Ssn { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public DateTime StartDateTime { get; set; }
        public string ExaminationRoom { get; set; }
    }
}
