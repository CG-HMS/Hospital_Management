using Hms.MVC.Models.Dashboard;
using Hms.MVC.Models.Patient;
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

    // ── Entry point — routes patient role to their own dashboard ──────────
    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("UserRole") ?? "guest";

        if (role == "patient")
            return await PatientDashboard();

        ViewData["Title"] = "Dashboard";

        var username = HttpContext.Session.GetString("Username") ?? "User";

        var model = new DashboardViewModel
        {
            Username = username,
            Role = role
        };

        try
        {
            var patientsTask      = _api.GetAsync<List<object>>("Patient");
            var roomsTask         = _api.GetAsync<List<RoomSummary>>("rooms");
            var stayCountTask     = _api.GetAsync<StayCountResponse>("Stay/stats/active-count");
            var appointmentsTask  = _api.GetAsync<List<AppointmentSummary>>("Appointment/today");

            await Task.WhenAll(patientsTask, roomsTask, stayCountTask, appointmentsTask);

            var patients     = await patientsTask;
            var rooms        = await roomsTask;
            var stayCount    = await stayCountTask;
            var appointments = await appointmentsTask;

            model.TotalPatients       = patients?.Count ?? 0;
            model.AvailableRooms      = rooms?.Count(r => !r.Unavailable) ?? 0;
            model.ActiveStays         = stayCount?.ActiveStaysCount ?? 0;
            model.TodayAppointments   = appointments?.Count ?? 0;
            model.RecentAppointments  = appointments ?? new();
            model.RoomSummary         = rooms ?? new();
        }
        catch
        {
            TempData["ErrorMessage"] = "Could not load some dashboard data. Is the API running?";
        }

        return View(model);
    }

    // ── Patient-only dashboard — all data scoped to the logged-in patient ──
    [Authorize(Roles = "patient")]
    public async Task<IActionResult> PatientDashboard()
    {
        ViewData["Title"] = "My Dashboard";

        var username = HttpContext.Session.GetString("Username") ?? "Patient";
        var refId    = HttpContext.Session.GetString("RefId");

        if (string.IsNullOrWhiteSpace(refId) || !int.TryParse(refId, out int ssn))
        {
            TempData["ErrorMessage"] = "Patient account is not linked to a profile. Contact admin.";
            return View("PatientDashboard", new PatientDashboardViewModel { Username = username });
        }

        var model = new PatientDashboardViewModel { Username = username };

        // Run all API calls in parallel — every endpoint is scoped to this patient's SSN
        var profileTask      = _api.GetAsync<PatientProfileDto>($"Patient/profile");
        var statsTask        = _api.GetAsync<PatientDashboardStatsDto>($"Patient/{ssn}/dashboard");
        var appointmentsTask = _api.GetAsync<List<PatientAppointmentDto>>($"Patient/{ssn}/appointments");
        var medicationsTask  = _api.GetAsync<List<PatientMedicationDto>>($"Patient/{ssn}/medications");
        var proceduresTask   = _api.GetAsync<List<PatientProcedureDto>>($"Patient/{ssn}/procedures");
        var staysTask        = _api.GetAsync<List<PatientStayDto>>($"Patient/{ssn}/stays");

        await Task.WhenAll(profileTask, statsTask, appointmentsTask,
                           medicationsTask, proceduresTask, staysTask);

        model.Profile      = await profileTask      ?? new PatientProfileDto { Name = username };
        model.Stats        = await statsTask        ?? new PatientDashboardStatsDto();
        model.Appointments = await appointmentsTask ?? new List<PatientAppointmentDto>();
        model.Medications  = await medicationsTask  ?? new List<PatientMedicationDto>();
        model.Procedures   = await proceduresTask   ?? new List<PatientProcedureDto>();
        model.Stays        = await staysTask        ?? new List<PatientStayDto>();

        return View("PatientDashboard", model);
    }
}
