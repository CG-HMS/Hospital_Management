using System.ComponentModel.DataAnnotations;

namespace Hms.API.DTOs;

public class StayDTO
{
    public int StayId { get; set; }
    public int Patient { get; set; }
    public int Room { get; set; }
    public DateTime StayStart { get; set; }
    public DateTime StayEnd { get; set; }
}

public class CreateStayDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be greater than 0")]
    public int Patient { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Room ID must be greater than 0")]
    public int Room { get; set; }

    [Required(ErrorMessage = "Stay start date is required")]
    public DateTime StayStart { get; set; }

    [Required(ErrorMessage = "Stay end date is required")]
    public DateTime StayEnd { get; set; }
}

public class UpdateStayDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be greater than 0")]
    public int Patient { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Room ID must be greater than 0")]
    public int Room { get; set; }

    [Required(ErrorMessage = "Stay start date is required")]
    public DateTime StayStart { get; set; }

    [Required(ErrorMessage = "Stay end date is required")]
    public DateTime StayEnd { get; set; }
}

public class StayDetailDTO
{
    public int StayId { get; set; }
    public int Patient { get; set; }
    public int Room { get; set; }
    public DateTime StayStart { get; set; }
    public DateTime StayEnd { get; set; }
    public string? PatientName { get; set; }
    public string? RoomNumber { get; set; }
    public int DaysOfStay { get; set; }
}

