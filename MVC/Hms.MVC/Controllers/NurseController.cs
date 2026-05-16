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
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NurseCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _api.PostAsync<NurseCreateViewModel>("Nurse", model);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }
}
