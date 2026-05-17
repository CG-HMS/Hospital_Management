using Hms.MVC.Models.Prescription;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize(Roles = "admin,physician,doctor,nurse")]
public class PrescriptionController : Controller
{
    private readonly IApiService _api;

    public PrescriptionController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var prescriptions = await _api.GetAsync<List<PrescriptionViewModel>>("Prescription");
        return View(prescriptions ?? new List<PrescriptionViewModel>());
    }
}
