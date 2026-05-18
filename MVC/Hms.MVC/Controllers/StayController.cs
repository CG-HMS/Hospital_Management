using Hms.MVC.Models.Stay;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize]
public class StayController : Controller
{
    private readonly IApiService _api;

    public StayController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var stays = await _api.GetAsync<List<StayViewModel>>("Stay");
        return View(stays ?? new List<StayViewModel>());
    }

    [HttpGet("Active")]
    public async Task<IActionResult> Active()
    {
        var stays = await _api.GetAsync<List<StayViewModel>>("Stay/active");
        return View("Index", stays ?? new List<StayViewModel>());
    }
}
