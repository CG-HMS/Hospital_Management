namespace Hms.MVC.Models.Dashboard;

public class DashboardViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    // Stats
    public int TotalPatients { get; set; }
    public int TodayAppointments { get; set; }
    public int AvailableRooms { get; set; }
    public int ActiveStays { get; set; }

    // Recent Data
    public List<AppointmentSummary> RecentAppointments { get; set; } = new();
    public List<RoomSummary> RoomSummary { get; set; } = new();
}

public class AppointmentSummary
{
    public int AppointmentId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string PhysicianName { get; set; } = string.Empty;
    public DateTime Starto { get; set; }
    public DateTime Endo { get; set; }
    public string ExaminationRoom { get; set; } = string.Empty;
}

public class RoomSummary
{
    public int RoomNumber { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public bool Unavailable { get; set; }
}

public class StayCountResponse
{
    public int ActiveStaysCount { get; set; }
    public DateTime Timestamp { get; set; }
}
