using Hms.MVC.Models.Dashboard;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IApiService _api;

    public DashboardController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";

        var username = HttpContext.Session.GetString("Username") ?? "User";
        var role = HttpContext.Session.GetString("UserRole") ?? "guest";

        var model = new DashboardViewModel
        {
            Username = username,
            Role = role
        };

        try
        {
            // Fetch stats in parallel
            var patientsTask = _api.GetAsync<List<object>>("Patient");
            var roomsTask = _api.GetAsync<List<RoomSummary>>("rooms");
            var stayCountTask = _api.GetAsync<StayCountResponse>("Stay/stats/active-count");
            var appointmentsTask = _api.GetAsync<List<AppointmentSummary>>("Appointment/today");

            await Task.WhenAll(patientsTask, roomsTask, stayCountTask, appointmentsTask);

            var patients = await patientsTask;
            var rooms = await roomsTask;
            var stayCount = await stayCountTask;
            var appointments = await appointmentsTask;

            model.TotalPatients = patients?.Count ?? 0;
            model.AvailableRooms = rooms?.Count(r => !r.Unavailable) ?? 0;
            model.ActiveStays = stayCount?.ActiveStaysCount ?? 0;
            model.TodayAppointments = appointments?.Count ?? 0;
            model.RecentAppointments = appointments ?? new();
            model.RoomSummary = rooms ?? new();
        }
        catch
        {
            // Dashboard should still render even if API is down
            TempData["ErrorMessage"] = "Could not load some dashboard data. Is the API running?";
        }

        return View(model);
    }
}
