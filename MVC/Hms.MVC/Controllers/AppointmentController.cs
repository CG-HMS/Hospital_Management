using Hms.MVC.Models.Appointment;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize(Roles = "admin,physician,nurse")]
public class AppointmentController : Controller
{
    private readonly IApiService _api;

    public AppointmentController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var appointments = await _api.GetAsync<List<AppointmentViewModel>>("Appointment");
        return View(appointments ?? new List<AppointmentViewModel>());
    }
}
