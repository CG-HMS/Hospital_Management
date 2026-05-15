using Hms.MVC.Models.Physician;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize(Roles = "admin")]
public class PhysicianController : Controller
{
    private readonly IApiService _api;

    public PhysicianController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var physicians = await _api.GetAsync<List<PhysicianViewModel>>("Physician");
        return View(physicians ?? new List<PhysicianViewModel>());
    }
}
