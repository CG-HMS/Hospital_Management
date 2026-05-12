namespace Hms.API.DTOs
{
    public class ProcedureDto
    {
        public int Code { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public decimal Cost { get; set; }
    }

    public class ProcedureWriteDto
    {
        public string? Name { get; set; }

        public decimal? Cost { get; set; }
    }
        public class ProcedurePhysicianDto
        {
            public int EmployeeId { get; set; }

            public string Name { get; set; }
                = string.Empty;

            public string Position { get; set; }
                = string.Empty;
        }
    public class StayDto
    {
        public int StayId { get; set; }

        public string PatientName { get; set; }
            = string.Empty;

        public int RoomNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

}