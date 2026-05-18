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

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AppointmentCreateViewModel
        {
            Starto = DateTime.Now,
            Endo = DateTime.Now.AddHours(1)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var response = await _api.PostAsync<AppointmentViewModel>("Appointment", new
        {
            patient = model.Patient,
            prepNurse = model.PrepNurse,
            physician = model.Physician,
            starto = model.Starto,
            endo = model.Endo,
            examinationRoom = model.ExaminationRoom
        });

        if (response != null)
        {
            TempData["SuccessMessage"] = "Appointment created successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = "Failed to create appointment. Please check the data and try again.";
        return View(model);
    }
}
