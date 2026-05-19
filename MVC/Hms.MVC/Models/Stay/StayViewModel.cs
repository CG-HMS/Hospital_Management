using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Stay;

public class StayViewModel
{
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int StayId { get; set; }

    [Required(ErrorMessage = "Patient ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Patient { get; set; }

    [Required(ErrorMessage = "Room ID is required")]
    [Range(0, int.MaxValue, ErrorMessage = "ID cannot be negative")]
    public int Room { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StayStart { get; set; }

    [Required(ErrorMessage = "End date is required")]
    public DateTime StayEnd { get; set; }
}
