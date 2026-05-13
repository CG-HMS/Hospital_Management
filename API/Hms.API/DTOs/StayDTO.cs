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
    public int Patient { get; set; }

    public int Room { get; set; }

    public DateTime StayStart { get; set; }

    public DateTime StayEnd { get; set; }
}

public class UpdateStayDTO
{
    public int Patient { get; set; }

    public int Room { get; set; }

    public DateTime StayStart { get; set; }

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

