using Hms.MVC.Models.Medication;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize]
public class MedicationController : Controller
{
    private readonly IApiService _api;

    public MedicationController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var medications = await _api.GetAsync<List<MedicationViewModel>>("Medication");
        return View(medications ?? new List<MedicationViewModel>());
    }
}
