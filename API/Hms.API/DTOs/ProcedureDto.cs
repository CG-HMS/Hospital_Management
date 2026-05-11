using System.ComponentModel.DataAnnotations;

namespace Hms.API.DTOs
{
    public class ProcedureDto
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
    public class CreateProcedureDto
    {
        public string Name { get; set; }
        public float Cost { get; set; }
    }
    public class UpdateProcedureDto
    {
        public string? Name { get; set; }
        public float? Cost { get; set; }
    }
    public class ProcedurePhysicianDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
    public class StayDto
    {
        public int StayId { get; set; }
        public string PatientName { get; set; }
        public int RoomNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
