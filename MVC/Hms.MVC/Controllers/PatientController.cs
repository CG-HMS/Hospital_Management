using Hms.MVC.Models.Patient;
using Hms.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hms.MVC.Controllers;

[Authorize(Roles = "admin,physician")]
public class PatientController : Controller
{
    private readonly IApiService _api;

    public PatientController(IApiService api)
    {
        _api = api;
    }

    // GET: Patient
    public async Task<IActionResult> Index()
    {
        var patients = await _api.GetAsync<List<PatientViewModel>>("Patient");
        return View(patients ?? new List<PatientViewModel>());
    }

    // GET: Patient/Create
    public IActionResult Create()
    {
        return View(new PatientViewModel());
    }

    // POST: Patient/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatientViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var response = await _api.PostAsync<PatientViewModel>("Patient", new
        {
            ssn = model.Ssn,
            name = model.Name,
            address = model.Address,
            phone = model.Phone,
            insuranceId = model.InsuranceId,
            pcp = model.Pcp
        });

        if (response != null)
        {
            TempData["SuccessMessage"] = "Patient created successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = "Failed to create patient. Please check the data and try again.";
        return View(model);
    }
    
    // POST: Patient/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _api.DeleteAsync($"Patient/{id}");
        
        if (success)
        {
            TempData["SuccessMessage"] = "Patient deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete patient. They might have associated records.";
        }
        
        return RedirectToAction(nameof(Index));
    }
}
