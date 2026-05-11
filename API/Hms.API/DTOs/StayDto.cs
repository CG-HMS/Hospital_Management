namespace Hms.API.DTOs
{
    public class StayDto
    {
        public int StayId { get; set; }

        public string PatientName { get; set; }

        public int RoomNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
