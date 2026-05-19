using System.ComponentModel.DataAnnotations;

namespace Hms.MVC.Models.Room;

public class RoomViewModel
{
    [Required(ErrorMessage = "Room number is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Room number cannot be negative")]
    public int RoomNumber { get; set; }

    [Required(ErrorMessage = "Room type is required")]
    [StringLength(50, ErrorMessage = "Room type cannot exceed 50 characters")]
    public string RoomType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Block floor is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Block floor cannot be negative")]
    public int BlockFloor { get; set; }

    [Required(ErrorMessage = "Block code is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Block code cannot be negative")]
    public int BlockCode { get; set; }

    public bool Unavailable { get; set; }
}
