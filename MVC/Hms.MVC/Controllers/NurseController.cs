using Hms.MVC.Models.Nurse;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize(Roles = "admin")]
public class NurseController : Controller
{
    private readonly IApiService _api;

    public NurseController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var nurses = await _api.GetAsync<List<NurseViewModel>>("Nurse");
        return View(nurses ?? new List<NurseViewModel>());
    }
}
