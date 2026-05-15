namespace Hms.API.DTOs.Patient;

public class PatientStayHistoryDto
{
    public int StayId { get; set; }
    public DateTime StayStart { get; set; }
    public DateTime StayEnd { get; set; }
    public int RoomNumber { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public int BlockFloor { get; set; }
    public int BlockCode { get; set; }
}
