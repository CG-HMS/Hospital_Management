namespace Hms.MVC.Models.Room;

public class RoomViewModel
{
    public int RoomNumber { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public int BlockFloor { get; set; }
    public int BlockCode { get; set; }
    public bool Unavailable { get; set; }
}
