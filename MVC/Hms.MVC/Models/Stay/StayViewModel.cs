namespace Hms.MVC.Models.Stay;

public class StayViewModel
{
    public int StayId { get; set; }
    public int Patient { get; set; }
    public int Room { get; set; }
    public DateTime StayStart { get; set; }
    public DateTime StayEnd { get; set; }
}
