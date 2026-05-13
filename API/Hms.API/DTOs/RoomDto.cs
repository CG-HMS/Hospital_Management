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

}