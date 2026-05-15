namespace Hms.API.DTOs
{
    public class RoomDto
    {
        public int RoomNumber { get; set; }
        public string RoomType { get; set; } = null!;
        public int BlockFloor { get; set; }
        public int BlockCode { get; set; }
        public bool Unavailable { get; set; }
    }

    public class RoomWriteDto
    {
        public string RoomType { get; set; } = null!;
        public int BlockFloor { get; set; }
        public int BlockCode { get; set; }
    }

    public class RoomPatientHistoryDto
    {
        public int StayId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime StayStart { get; set; }
        public DateTime StayEnd { get; set; }
    }

    public class RoomUtilizationDto
    {
        public int RoomNumber { get; set; }
        public int TotalStays { get; set; }
        public DateTime? LastStayEnd { get; set; }
    }

}